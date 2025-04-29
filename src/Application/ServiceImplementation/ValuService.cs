using Microsoft.AspNetCore.Http;
using WaffarXPartnerApi.Application.Common.DTOs.Helper;
using WaffarXPartnerApi.Application.Common.DTOs.Shared.ExitClick;
using WaffarXPartnerApi.Application.Common.DTOs.ValuRequestDto;
using WaffarXPartnerApi.Application.Common.DTOs.ValuResponseDto;
using WaffarXPartnerApi.Application.Common.Models.SharedModels;
using WaffarXPartnerApi.Application.ServiceImplementation.Shared;
using WaffarXPartnerApi.Application.ServiceInterface;
using WaffarXPartnerApi.Domain.Enums;
using WaffarXPartnerApi.Domain.Models.SharedModels;
using WaffarXPartnerApi.Domain.RepositoryInterface.EntityFrameworkRepositoryInterface;

namespace WaffarXPartnerApi.Application.ServiceImplementation;
public class ValuService : BaseService, IValuService
{
    private readonly IHttpService _httpService;
    private readonly IApiClientRepository _apiClientRepository;

    public ValuService(IHttpService httpService, IHttpContextAccessor httpContextAccessor
        , IApiClientRepository apiClientRepository) :base(httpContextAccessor)
    {
        _httpService = httpService;
        _apiClientRepository = apiClientRepository;
    }

    public async Task<GenericResponse<List<ProductSearchResultDto>>> GetFeaturedProducts(GetFeaturedProductDto product)
    {
        var headers = new Dictionary<string, string>
        {
            ["Content-Type"] = "application/json"
        };
        GetFeaturedProductRequestDto requestObj = new GetFeaturedProductRequestDto 
        {
            ClientApiId = ClientApiId,
            Count = product.Count,
            PageNumber = product.PageNumber,
            IsEnglish = IsEnglish,
        };
        // Make the POST request using our generic HTTP service
        var searchResults = await _httpService.PostAsync<GenericResponse<List<ProductSearchResponseModel>>>(
            AppSettings.ExternalApis.ValuUrl + "GetFeaturedProducts",
            requestObj,
            headers);
        if (searchResults.Status == StaticValues.Success && searchResults.Data.Any()) 
        {
            List<ProductSearchResultDto> products = new List<ProductSearchResultDto>();
            SearchFilterDto filter = new SearchFilterDto();
            foreach (var item in searchResults.Data)
            {
                products.Add(MapToProduct(item));
            }

            return new GenericResponse<List<ProductSearchResultDto>>
            {
                Status = StaticValues.Success,
                Data =  products 
            };
        }
        return new GenericResponse<List<ProductSearchResultDto>>();
    }

    public async Task<GenericResponse<ProductSearchResultDto>> GetProductDetails(string id)
    {
        try
        {
            var headers = new Dictionary<string, string>
            {
                ["Content-Type"] = "application/json"
            };
            ProductById product = new ProductById
            {
                IsEnglish = IsEnglish,
                ProductId = id
            };

            // Make the POST request using our generic HTTP service
            var searchResults = await _httpService.PostAsync<ProductSearchResponseModel>(
                AppSettings.ExternalApis.ValuUrl + "product",
                product,
                headers);
            if (searchResults != null)
            {
                return new GenericResponse<ProductSearchResultDto>
                {
                    Data = MapToProduct(searchResults),
                    Status = StaticValues.Success
                };
            }
          
            return new GenericResponse<ProductSearchResultDto>();
        }
        catch(Exception)
        {
            throw;
        }
    }

    public async Task<GenericResponse<StoreDetailDto>> GetStoreDetails(Guid storeId)
    {
        var headers = new Dictionary<string, string>
        {
            ["Content-Type"] = "application/json"
        };

        GetStoreDto store = new GetStoreDto
        {
            ClientApiId = ClientApiId,
            IsEnglish = IsEnglish,
            StoreId = storeId
        };

        // Make the POST request using our generic HTTP service
        var searchResults = await _httpService.PostAsync<GenericResponse<StoreDetailModel>>(
            AppSettings.ExternalApis.ValuUrl + "GetStoreDetails",
            store,
            headers);
        if (searchResults.Status == StaticValues.Success && searchResults.Data != null) 
        {
            List<ProductSearchResultDto> products = new List<ProductSearchResultDto>();
            foreach (var item in searchResults.Data.StoreFeaturedProducts)
            {
                products.Add(MapToProduct(item));
            }
            return new GenericResponse<StoreDetailDto>
            {
                Status = StaticValues.Success,
                Data = new StoreDetailDto
                {
                    StoreFeaturedProducts = products,
                    Store = new StoreDto
                    {
                        Id = searchResults.Data.Store.Id,
                        Logo = searchResults.Data.Store.Logo,
                        Name = searchResults.Data.Store.Name,
                        LogoPng = searchResults.Data.Store.LogoPng,
                    }
                }
            };
        }
        return new GenericResponse<StoreDetailDto>();
    }

    public async Task<GenericResponse<List<StoreDto>>> GetStores(GetStoresRequestDto model)
    {
        var headers = new Dictionary<string, string>
        {
            ["Content-Type"] = "application/json"
        };
        GetStoresDto requestBody = new GetStoresDto  
        {
            ClientApiId = ClientApiId,
            IsEnglish = IsEnglish,
            ItemCount = model.ItemCount,
            PageNumber = model.PageNumber,
            
        };
        // Make the POST request using our generic HTTP service
        var searchResults = await _httpService.PostAsync<GenericResponse<List<StoreDto>>>(
            AppSettings.ExternalApis.ValuUrl + "GetStores",
            requestBody,
            headers);
        if(searchResults.Status == StaticValues.Success && searchResults.Data != null)
        {
            return new GenericResponse<List<StoreDto>>
            {
                Status = StaticValues.Success,
                Data = searchResults.Data,
                TotalCount = searchResults.TotalCount,
            };
        }
        return new GenericResponse<List<StoreDto>>();
    }

    public async Task<GenericResponse<ProductSearchResultWithFiltersDto>> SearchProduct(ProductSearchRequestDto productSearch)
    {
        try
        {
            ProductSearchResultWithFiltersDto response = new ProductSearchResultWithFiltersDto();

            // Set up any headers you need
            var headers = new Dictionary<string, string>
            {
                ["Content-Type"] = "application/json"
            };
            ProductSearchDto requestBody = new ProductSearchDto
            {
                
                Filter = new FilterDto
                {
                    Brands = productSearch?.Filter?.Brands,
                    Stores = productSearch?.Filter?.Stores,
                    MinPrice = productSearch?.Filter?.MinPrice,
                    MaxPrice = productSearch?.Filter?.MaxPrice,
                    Offers = productSearch?.Filter?.Offers
                    
                },
                ClientApiId = ClientApiId,
                IsEnglish = IsEnglish,
                PageNumber = productSearch.PageNumber,
                ItemCount = productSearch.ItemCount,
                SearchText = productSearch.SearchText,

            };

            // Make the POST request using our generic HTTP service
            var searchResults = await _httpService.PostAsync<GenericResponse<ValuSearchResponseDto>>(
                AppSettings.ExternalApis.ValuUrl + "search",
                requestBody,
                headers);
            if (searchResults.Data != null && searchResults.Data.Products.Any())
            {
                List<ProductSearchResultDto> products = new List<ProductSearchResultDto>();
                SearchFilterDto filter = new SearchFilterDto();
                foreach (var product in searchResults.Data.Products)
                {
                    products.Add(MapToProduct(product));
                }

                return new GenericResponse<ProductSearchResultWithFiltersDto>
                {
                    Status = StaticValues.Success,
                    Data = new ProductSearchResultWithFiltersDto
                    {
                        Products = products,
                        Filters = new SearchFilterDto
                        {
                            Brands = searchResults.Data.Filters?.Brands,
                            Stores = searchResults.Data.Filters?.Stores,
                            MinPrice = searchResults.Data.Filters.MinPrice,
                            MaxPrice = searchResults.Data.Filters.MaxPrice,
                            Offers = searchResults.Data.Filters?.Offers,
                        }
                    }
                };

            }
            return new GenericResponse<ProductSearchResultWithFiltersDto>();
        }
        catch (Exception)
        {
            throw;
        }
    }

    private ProductSearchResultDto MapToProduct(ProductSearchResponseModel model)
    {
        try
        {

            if (model == null)
                return null;
            ProductSearchResultDto res = new ProductSearchResultDto 
            {
                Id = model.Id,
                Name = model.Name,
                Price = model.Price,
                Description = model.Description,
                Brand = model.Brand,
                Currency = model.Currency,
                PrimaryImg = model.PrimaryImg,
                VariantsImgs = model.VariantsImgs,
                Category = model.Category,
                Discounted = model.Discounted,
                DiscountedText = model.DiscountedText,
                ErrorImg = model.ErrorImg,
                Feature = model.Features,
                OldPrice = model.OldPrice,
                OldPriceText = model.OldPriceText,
                PriceText = model.PriceText,
                SKU = model.SKU,
                Warranty = model.Warranty,
                Specification = model.Specification?.ToDictionary(k => k.Key, v => (object)v),
                PriceVariants = model.price_variants?.Select(v => new PriceVariantDto
                {
                    VariantId = v.variant_id,
                    Price = v.price,
                    Sku = v.sku,
                    Title = v.title,
                    Available = v.available,
                    Options = v.options
                }).ToList(),
                Options = (model.options as IEnumerable<OptionDto>)?.Select(o => new OptionDto
                {
                    Name = o.Name,
                    Id = o.Id,
                    ProductId = o.ProductId,
                    Position = o.Position,
                    Values = o.Values
                }).ToList(),
                Store = model.Store == null ? new StoreDto() :  
                new StoreDto
                {
                    Id = (Guid)(model.Store?.Id),
                    Logo = model?.Store?.Logo,
                    Name = model?.Store?.Name,
                    LogoPng = model?.Store?.LogoPng,
                },
                Offers = model?.Offers,
                
            };
            return res;

        }
        catch(Exception)
        {
            throw;
        }
      
    }

    public async Task<GenericResponse<int>> CreateExitClick(string section, Guid storeId, string productId = "")
    {
        try
        {
            // Get All Data Needed
            long UserId = await _apiClientRepository.GetUserIdByClient(ClientApiId.ToString());
            // Create ExitClick Request Data
            CreateExitClickRequestDto requestBody = RequestDataHelper.CreateExitClickRequestData((int)UserId
                , (int)ExitClickSourcesEnum.Valu, IsEnglish, section, storeId, productId);

            var headers = new Dictionary<string, string>
            {
                ["Content-Type"] = "application/json"
            };
            
            // Make the POST request using our generic HTTP service
            var searchResults = await _httpService.PostAsync<GenericResponse<int>>(
                AppSettings.ExternalApis.SharedApiUrl + "ExitClick/CreateExitClick",
                requestBody,
                headers);
            if (searchResults.Status == StaticValues.Success && searchResults.Data > 0)
            {
                return new GenericResponse<int>
                {
                    Status = StaticValues.Success,
                    Data = searchResults.Data,
                    TotalCount = searchResults.TotalCount,
                };
            }
            return new GenericResponse<int>()
            {
                Status = StaticValues.Success,
                Data =  1,  
            };
        }
        catch (Exception)
        {
            throw;
        }
    }

}
