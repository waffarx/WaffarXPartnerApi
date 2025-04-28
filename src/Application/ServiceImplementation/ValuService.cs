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

    public async Task<ProductSearchResultDto> GetProductDetails(ProductById product)
    {
        try
        {
            var headers = new Dictionary<string, string>
            {
                ["Content-Type"] = "application/json"
            };
            product.IsEnglish = IsEnglish;

            // Make the POST request using our generic HTTP service
            var searchResults = await _httpService.PostAsync<ProductSearchResponseModel>(
                AppSettings.ExternalApis.ValuUrl + "/product",
                product,
                headers);
            if (searchResults != null)
            {
                return MapToProduct(searchResults);
            }
          
            return new ProductSearchResultDto();
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
        store.ClientApiId = ClientApiId;
        store.IsEnglish = IsEnglish;


        // Make the POST request using our generic HTTP service
        var searchResults = await _httpService.PostAsync<GenericResponse<StoreDetailsDto>>(
            AppSettings.ExternalApis.ValuUrl + "/GetStoreDetails",
            store,
            headers);

        return new Guid();
    }

    public async Task<Guid> GetStores()
    {
        var headers = new Dictionary<string, string>
        {
            ["Content-Type"] = "application/json"
        };
        var requestBody = new 
        {
            ClientApiId = ClientApiId,
            IsEnglish = IsEnglish
        };
        // Make the POST request using our generic HTTP service
        var searchResults = await _httpService.PostAsync<GenericResponse<List<StoreDto>>>(
            AppSettings.ExternalApis.ValuUrl + "/GetStores",
            requestBody,
            headers);
        return new Guid();
    }

    public async Task<List<ProductSearchResultDto>> SearchProduct(ProductSearchRequestDto productSearch)
    {
        try
        {

            // Set up any headers you need
            var headers = new Dictionary<string, string>
            {
                ["Content-Type"] = "application/json"
            };
            productSearch.ClientApiId = ClientApiId;
            productSearch.IsEnglish = IsEnglish;

            // Make the POST request using our generic HTTP service
            var searchResults = await _httpService.PostAsync<GenericResponse<ValuSearchResponseDto>>(
                AppSettings.ExternalApis.ValuUrl + "/search",
                productSearch,
                headers);
            if (searchResults.Data != null && searchResults.Data.Products.Any())
            {
                return MapToProducts(searchResults.Data.Products).ToList();
            }
                return  new List<ProductSearchResultDto>();
        }
        catch(Exception)
        {
            throw;
        }
    }

    private ProductSearchResultDto MapToProduct(ProductSearchResponseModel model)
    {
        if (model == null)
            return null;

        return new ProductSearchResultDto
        {
            Id = model.Id,
            Name = model.Name,
            Price = model.Price,
            Description = model.Description,
            Brand = model.Brand,
            Currency = model.Currency,
            PrimaryImg = model.PrimaryImg,
            VariantsImgs = model.VariantsImgs,
            PriceVariants = model.price_variants?.Select(v => new PriceVariantDto
            {
                VariantId = v.variant_id,
                Price = v.price,
                Sku = v.sku,
                Title = v.title,
                Available = v.available,
                InventoryQuantity = v.inventory_quantity,
                Options = v.options
            }).ToList(),
            Specification = model.Specification?.ToDictionary(k => k.Key, v => (object)v),
            Category = model.Category,
            Discounted = model.Discounted,
            DiscountedText = model.DiscountedText,
            ErrorImg = model.ErrorImg,
            Feature = model.Features,
            OldPrice = model.OldPrice,
            OldPriceText = model.OldPriceText,
            Options = (model.options as IEnumerable<OptionDto>)?.Select(o => new OptionDto
            {
                Name = o.Name,
                Id = o.Id,
                ProductId = o.ProductId,
                Position = o.Position,
                Values = o.Values
            }).ToList(),
            ShoppingURL = model.ExitClickURL,
            PriceText = model.PriceText,
            SKU = model.SKU,
            Store = new StoreDto
            {
                Id = model.Store.Id,
                Logo = model.Store.Logo,
                Name = model.Store.Name,
                LogoPng = model.Store.LogoPng,
            },
            Warranty = model.Warranty,
            
            
        };
    }
    private IEnumerable<ProductSearchResultDto> MapToProducts(IEnumerable<ProductSearchResponseModel> models)
    {
        if (models == null)
            return Enumerable.Empty<ProductSearchResultDto>();

        return models.Select(MapToProduct).Where(p => p != null);
    }
}
