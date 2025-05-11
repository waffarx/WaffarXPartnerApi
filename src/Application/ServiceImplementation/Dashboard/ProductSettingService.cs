using Microsoft.AspNetCore.Http;
using MongoDB.Bson;
using MongoDB.Driver;
using WaffarXPartnerApi.Application.Common.DTOs.Dashboard.Products.AddFeaturedProduct;
using WaffarXPartnerApi.Application.Common.DTOs.Dashboard.Products.DeleteFeatured;
using WaffarXPartnerApi.Application.Common.DTOs.Dashboard.Products.FeaturedProducts;
using WaffarXPartnerApi.Application.Common.DTOs.Dashboard.Products.FeaturedProducts.GetFeaturedProduct;
using WaffarXPartnerApi.Application.Common.DTOs.Dashboard.Products.RankFeaturedProducts;
using WaffarXPartnerApi.Application.Common.DTOs.Dashboard.Products.UpdateFeatured;
using WaffarXPartnerApi.Application.Common.DTOs.Valu.ValuRequestDto;
using WaffarXPartnerApi.Application.Common.DTOs.ValuRequestDto;
using WaffarXPartnerApi.Application.Common.DTOs.ValuResponseDto;
using WaffarXPartnerApi.Application.Common.Models.SharedModels;
using WaffarXPartnerApi.Application.Helper;
using WaffarXPartnerApi.Application.ServiceImplementation.Shared;
using WaffarXPartnerApi.Application.ServiceInterface.Dashboard;
using WaffarXPartnerApi.Application.ServiceInterface.Shared;
using WaffarXPartnerApi.Domain.Models.PartnerMongoModels;
using WaffarXPartnerApi.Domain.Models.PartnerMongoModels.FeaturedProducts;
using WaffarXPartnerApi.Domain.Models.SharedModels;
using WaffarXPartnerApi.Domain.RepositoryInterface.EntityFrameworkRepositoryInterface;
using WaffarXPartnerApi.Domain.RepositoryInterface.EntityFrameworkRepositoryInterface.WaffarX;
using WaffarXPartnerApi.Domain.RepositoryInterface.MongoRepositoryInterface;

namespace WaffarXPartnerApi.Application.ServiceImplementation.Dashboard;
public class ProductSettingService : JWTUserBaseService, IProductSettingService
{
    private readonly IAdvertiserRepository _advertiserRepository;
    private readonly IApiClientRepository _apiClientRepository;
    private readonly IPartnerRepository _partnerRepository; 
    private readonly IHttpService _httpService;
    private readonly ICacheService _cacheService;
    private readonly IMapper _mapper;
    public ProductSettingService(IMongoDatabase database, IAdvertiserRepository advertiserRepository, IPartnerRepository partnerRepository, IApiClientRepository apiClientRepository
        , ICacheService cacheService, IHttpService httpService, IMapper mapper, IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
    {
        _partnerRepository = partnerRepository; 
        _apiClientRepository = apiClientRepository;
        _httpService = httpService;
        _cacheService = cacheService;
        _advertiserRepository = advertiserRepository;
        _mapper = mapper;
    }
    public async Task<GenericResponseWithCount<List<FeaturedProductResponseDto>>> GetFeaturedProducts(GetPartnerFeaturedProductDto featuredProductDto)
    {
        List<FeaturedProductResponseDto> featuredProducts = new List<FeaturedProductResponseDto>(); 
        try
        {
            var clientGuid = await _apiClientRepository.GetClientGuidById(ClientApiId);
            // Get Chached
            string whiteListedStoresCacheKey = $"WhiteListedStores:{clientGuid}";
            string disabledStoresCacheKey = "DisabledStores";

            var disabledStores = await _cacheService.GetOrSetCacheValueAsync(
                disabledStoresCacheKey,
                () => _advertiserRepository.GetDisabledStores(), TimeSpan.FromHours(12));

            var WhiteListedStores = await _cacheService.GetOrSetCacheValueAsync(
                     whiteListedStoresCacheKey, () => _partnerRepository.GetWhiteListStores(ClientApiId, disabledStores), TimeSpan.FromHours(24));
            
            var ClientFeaturedList = await _partnerRepository.GetFeaturedProducts(ClientApiId, WhiteListedStores, featuredProductDto.IsActive, featuredProductDto.PageNumber, featuredProductDto.PageSize);
            if (ClientFeaturedList != null && ClientFeaturedList.TotalRecords > 0 && ClientFeaturedList.Data?.Count > 0)
            {
                var productIds = ClientFeaturedList.Data.Select(x => x.ProductId).ToList();
                if (productIds.Count > 0)
                { 
                    var productsDetails = await GetProductsByIds(productIds, clientGuid);
                    if (productsDetails?.Count > 0)
                    {
                        foreach (var product in ClientFeaturedList.Data)
                        {
                            var productDetails = productsDetails.FirstOrDefault(x => x.Id == product.ProductId.ToString());
                            FeaturedProductResponseDto featured = new FeaturedProductResponseDto()
                            {
                                ProductId = product.ProductId.ToString(),
                                Rank = product.ProductRank,
                                StartDate = product.StartDate,
                                EndDate = product.EndDate,
                                ProductDetails = productDetails != null ? ProductMappingHelper.MapToBaseProduct(productDetails) : null
                            };
                            if (featured.ProductDetails != null)
                            { 
                                featuredProducts.Add(featured);
                            }
                        }
                    }
                }
                return new GenericResponseWithCount<List<FeaturedProductResponseDto>>
                {
                    Status = StaticValues.Success,
                    Data = featuredProducts,
                    TotalCount = ClientFeaturedList.TotalRecords,
                };
            }
            return new GenericResponseWithCount<List<FeaturedProductResponseDto>>()
            {
                Data = new List<FeaturedProductResponseDto>(),
                Status = StaticValues.Error,
                TotalCount = 0,
            };
        }
        catch (Exception)
        {
            throw;
        }
    }
    private async Task<List<ProductSearchResponseModel>> GetProductsByIds(List<ObjectId> productIds, Guid clientGuid)
    {
        try
        {
            var headers = new Dictionary<string, string>
            {
                ["Content-Type"] = "application/json"
            };

            GetProductDetailsRequestDto requestBody = new GetProductDetailsRequestDto
            {
                ClientApiId = clientGuid,
                IsEnglish = IsEnglish,
                Products = productIds?.Select(id => id.ToString()).ToList()
            };
            // Make the POST request using our generic HTTP service
            var searchResults = await _httpService.PostAsync<GenericResponse<ValuSearchResponseDto>>(
                AppSettings.ExternalApis.ValuUrl + "GetProductDetails",
                requestBody,
                headers);
            if (searchResults.Status == StaticValues.Success && searchResults.Data != null)
            {
                return searchResults.Data.Products;
            }
            return new List<ProductSearchResponseModel>();
        }
        catch (Exception)
        {
            throw;
        }
        
    }
    public async Task<GenericResponse<bool>> DeleteFeaturedProduct(DeleteFeaturedProductDto productDto)
    {
        GenericResponse<bool> response = new GenericResponse<bool>();   
        try
        {
            if (!ObjectId.TryParse(productDto.productId, out ObjectId objectId))
            {

                response.Status = StaticValues.Error;
                response.Data = false;
                response.Errors = new List<string> { "Invalid product" };
                return response; 
            }
            bool isDeleted = await _partnerRepository.DeleteFeaturedProductById(objectId, ClientApiId, UserIdInt);
            if (isDeleted) 
            {
                response.Status = StaticValues.Success;
                response.Data = true;
                response.Message = "Product Deleted Successfully";
                return response;
            }
            response.Status = StaticValues.Error;
            response.Data = false;
            response.Errors = new List<string> { "Invalid product" };
            return response;
        }
        catch (Exception)
        {
            throw;
        }
    }
    public async Task<GenericResponse<bool>> UpdateFeaturedProduct(UpdateFeaturedProductDto productDto)
    {
        GenericResponse<bool> response = new GenericResponse<bool>();
        try
        {
            if (!ObjectId.TryParse(productDto.ProductId, out ObjectId objectId))
            {

                response.Status = StaticValues.Error;
                response.Data = false;
                response.Errors = new List<string> { "Invalid product" };
                return response;
            }
            UpdateFeaturedProductModel model = _mapper.Map<UpdateFeaturedProductModel>(productDto);
            model.UserId = UserIdInt;
            model.ClientApiId = ClientApiId;
            bool isUpdated = await _partnerRepository.UpdateFeaturedProduct(model);
            if (isUpdated)
            {
                response.Status = StaticValues.Success;
                response.Data = true;
                response.Message = "Product Saved Successfully";
                return response;
            }
            response.Status = StaticValues.Error;
            response.Data = false;
            response.Errors = new List<string> { "Invalid product" };
            return response;
        }
        catch (Exception)
        {
            throw;
        }
    }
    public async Task<GenericResponse<bool>> AddFeaturedProductList(List<AddFeaturedProductDto> products)
    {
        try
        {
            // Get Max Active Featured Product Rank
            var maxRank = await _partnerRepository.GetActiveFeaturedProductMaxRank(ClientApiId);

            // Get All Store Ids from List of Store Guids
            var storeIds = await _partnerRepository.GetStoreIdsByGuids(products.Select(x => x.StoreId).ToList());

            // Map List of AddFeaturedProductDto to List of AddFeaturedProductModel
            List<AddFeaturedProductModel> featuredProducts = new List<AddFeaturedProductModel>();
            foreach (var product in products)
            {
                maxRank = maxRank + 1;
                AddFeaturedProductModel featuerd = new AddFeaturedProductModel()
                {
                    ClientApiId = ClientApiId,
                    UserId = UserIdInt,
                    StoreId  = product.StoreId,
                    StoreIdInt = storeIds.FirstOrDefault(x => x.StoreGuid.ToString() == product.StoreId)?.StoreId ?? 0,
                    ProductRank = maxRank,
                    EndDate = product.EndDate,
                    ProductId = ObjectId.Parse(product.ProductId),  
                    StartDate = product.StartDate
                };
                featuredProducts.Add(featuerd); 
            }
            // Add Featured Product List to Database
            bool isAdded = await _partnerRepository.AddFeaturedProductList(featuredProducts);
            
            return  new GenericResponse<bool>() 
            {
                Status = isAdded ? StaticValues.Success : StaticValues.Error,
                Data = isAdded,
                Message = isAdded ? "Product Added Successfully" : "Failed to Add Product",
                Errors = isAdded ? null : new List<string> { "Failed to Add Products" }
            };
        }
        catch (Exception)
        {

            throw;
        }
       
        
    }
    public async Task<GenericResponse<bool>> SaveFeaturedProductRank(List<RankProductsDto> products)
    {
        GenericResponse<bool> response = new GenericResponse<bool>();
        try
        {
            List<FeaturedProductsBase> featuredProducts = products.Select(x => new FeaturedProductsBase
            {
                ClientApiId = ClientApiId, 
                ProductId = ObjectId.Parse(x.ProductId),    
                ProductRank = x.ProductRank,
                UserId  = UserIdInt,  
            }).ToList();
            bool result = await _partnerRepository.SaveProductsRank(ClientApiId, UserIdInt, featuredProducts);
            if(result)
            {
                response.Status = StaticValues.Success;
                response.Data = true;
                response.Message = "Product Rank Saved Successfully";
                return response;
            }
            response.Status = StaticValues.Error;
            response.Data = true;
            response.Errors = new List<string> { "Failed to save product ranks" };
            return response;
        }
        catch (Exception)
        {

            throw;
        }
    }
}
