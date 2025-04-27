using Microsoft.AspNetCore.Http;
using WaffarXPartnerApi.Application.Common.DTOs.ValuRequestDto;
using WaffarXPartnerApi.Application.Common.DTOs.ValuResponseDto;
using WaffarXPartnerApi.Application.Common.Models.SharedModels;
using WaffarXPartnerApi.Application.ServiceImplementation.Shared;
using WaffarXPartnerApi.Application.ServiceInterface;

namespace WaffarXPartnerApi.Application.ServiceImplementation;
public class ValuService : BaseService, IValuService
{
    private readonly IHttpService _httpService;

    public ValuService(IHttpService httpService, IHttpContextAccessor httpContextAccessor):base(httpContextAccessor)
    {
        _httpService = httpService;
    }

    public async Task<Guid> GetFeaturedProducts(GetFeaturedProductDto product)
    {
        var headers = new Dictionary<string, string>
        {
            ["Content-Type"] = "application/json"
        };
        product.ClientApiId = ClientApiId;
        product.IsEnglish = IsEnglish;
        // Make the POST request using our generic HTTP service
        var searchResults = await _httpService.PostAsync<GenericResponse<List<ProductSearchResponseModel>>>(
            AppSettings.ExternalApis.ValuUrl + "/GetFeaturedProducts",
            product,
            headers);

        return new Guid();
    }

    public async Task<Guid> GetProductDetails(ProductById product)
    {
        try
        {
            var headers = new Dictionary<string, string>
            {
                ["Content-Type"] = "application/json"
            };

            // Make the POST request using our generic HTTP service
            var searchResults = await _httpService.PostAsync<ProductSearchResponseModel>(
                AppSettings.ExternalApis.ValuUrl + "/product",
                product,
                headers);

            return new Guid();
        }
        catch(Exception)
        {
            throw;
        }
    }

    public async Task<Guid> GetStoreDetails(GetStoreDto store)
    {
        var headers = new Dictionary<string, string>
        {
            ["Content-Type"] = "application/json"
        };

        // Make the POST request using our generic HTTP service
        var searchResults = await _httpService.PostAsync<GenericResponse<StoreDetailsDto>>(
            AppSettings.ExternalApis.ValuUrl + "/GetStoreDetails",
            store,
            headers);

        return new Guid();
    }

    public async Task<Guid> SearchProduct(ProductSearchDto productSearch)
    {
        try
        {

            // Set up any headers you need
            var headers = new Dictionary<string, string>
            {
                ["Content-Type"] = "application/json"
            };

            // Make the POST request using our generic HTTP service
            var searchResults = await _httpService.PostAsync<List<ProductSearchResponseModel>>(
                AppSettings.ExternalApis.ValuUrl + "/search",
                productSearch,
                headers);
            return new Guid();
        }
        catch(Exception)
        {
            throw;
        }
    }
}
