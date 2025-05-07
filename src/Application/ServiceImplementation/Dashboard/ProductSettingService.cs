using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using MongoDB.Bson;
using MongoDB.Driver;
using Sentry.Protocol;
using WaffarXPartnerApi.Application.Common.DTOs.Dashboard.Products.FeaturedProducts;
using WaffarXPartnerApi.Application.Common.DTOs.Dashboard.Products.FeaturedProducts.GetFeaturedProduct;
using WaffarXPartnerApi.Application.Common.DTOs.Valu.ValuRequestDto;
using WaffarXPartnerApi.Application.Common.DTOs.Valu.ValuRequestDto.GetFeaturedProductRequest;
using WaffarXPartnerApi.Application.Common.DTOs.ValuResponseDto;
using WaffarXPartnerApi.Application.Common.Models.SharedModels;
using WaffarXPartnerApi.Application.Helper;
using WaffarXPartnerApi.Application.ServiceImplementation.Shared;
using WaffarXPartnerApi.Application.ServiceInterface;
using WaffarXPartnerApi.Application.ServiceInterface.Dashboard;
using WaffarXPartnerApi.Application.ServiceInterface.Shared;
using WaffarXPartnerApi.Domain.Entities.NoSqlEnitities;
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
    public ProductSettingService(IMongoDatabase database
        , IAdvertiserRepository advertiserRepository
        , IPartnerRepository partnerRepository
        , IApiClientRepository apiClientRepository
        , ICacheService cacheService
        , IHttpService httpService
        , IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
    {
        _partnerRepository = partnerRepository; 
        _apiClientRepository = apiClientRepository;
        _httpService = httpService;
        _cacheService = cacheService;
        _advertiserRepository = advertiserRepository;
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
            
            var ClientFeaturedList = await _partnerRepository.GetFeaturedProducts(ClientApiId, WhiteListedStores, false ,featuredProductDto.PageNumber, featuredProductDto.PageSize);
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
}
