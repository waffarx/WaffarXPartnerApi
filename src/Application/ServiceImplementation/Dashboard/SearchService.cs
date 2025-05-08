using WaffarXPartnerApi.Application.Common.DTOs.Valu.SharedModels;
using WaffarXPartnerApi.Application.Common.DTOs.Valu.ValuRequestDto.ProductSearchRequest;
using WaffarXPartnerApi.Application.Common.DTOs.ValuResponseDto;
using WaffarXPartnerApi.Application.Common.Models.SharedModels;
using WaffarXPartnerApi.Application.Helper;
using WaffarXPartnerApi.Application.ServiceImplementation.Shared;
using WaffarXPartnerApi.Application.ServiceInterface;
using WaffarXPartnerApi.Application.ServiceInterface.Dashboard;
using WaffarXPartnerApi.Domain.Models.SharedModels;
using WaffarXPartnerApi.Domain.RepositoryInterface.EntityFrameworkRepositoryInterface.WaffarX;
using WaffarXPartnerApi.Domain.RepositoryInterface.EntityFrameworkRepositoryInterface;
using Microsoft.AspNetCore.Http;


namespace WaffarXPartnerApi.Application.ServiceImplementation.Dashboard;
public class SearchService : JWTUserBaseService, ISearchService
{
    private readonly IHttpService _httpService;
    private readonly IApiClientRepository _apiClientRepository;
    private readonly IAdvertiserRepository _advertiserRepository;
    public SearchService(IHttpService httpService, IHttpContextAccessor httpContextAccessor
       , IApiClientRepository apiClientRepository, IAdvertiserRepository advertiserRepository) : base(httpContextAccessor)
    {
        _httpService = httpService;
        _apiClientRepository = apiClientRepository;
        _advertiserRepository = advertiserRepository;
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
            SearchFilterModel filterModel = null;
            if (productSearch.Filter != null)
            {
                List<int> storeIds = new List<int>();
                if (productSearch.Filter.Stores?.Count > 0)
                {
                    List<Guid> guids = productSearch.Filter.Stores.Select(sg => new Guid(sg)).ToList();
                    storeIds = await _advertiserRepository.GetStoreIds(guids);
                }
                filterModel = new SearchFilterModel
                {
                    Brands = productSearch?.Filter?.Brand,
                    Stores = storeIds,
                    MinPrice = productSearch?.Filter?.MinPrice,
                    MaxPrice = productSearch?.Filter?.MaxPrice,
                    OfferId = productSearch?.Filter?.OfferId
                };

            }
            var clientGuid = await _apiClientRepository.GetClientGuidById(ClientApiId);
            ProductSearchDto requestBody = new ProductSearchDto
            {

                ClientApiId = clientGuid,
                IsEnglish = IsEnglish,
                PageNumber = productSearch.PageNumber,
                ItemCount = productSearch.PageSize,
                SearchText = productSearch.SearchText,
                Filter = filterModel

            };

            // Make the POST request using our generic HTTP service
            var searchResults = await _httpService.PostAsync<GenericResponse<ValuSearchResponseDto>>(
                AppSettings.ExternalApis.ValuUrl + "search",
                requestBody,
                headers);
            if (searchResults.Data != null && searchResults.Data.Products.Any())
            {
                List<BaseProductSearchResultDto> products = new List<BaseProductSearchResultDto>();
                SearchFilterDto filter = new SearchFilterDto();
                foreach (var product in searchResults.Data.Products)
                {
                    products.Add(ProductMappingHelper.MapToBaseProduct(product));
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
            return new GenericResponse<ProductSearchResultWithFiltersDto>
            {
                Data = new ProductSearchResultWithFiltersDto() { },
                Status = StaticValues.Error
            };
        }
        catch (Exception)
        {
            throw;
        }
    }
}
