using Microsoft.AspNetCore.Http;
using WaffarXPartnerApi.Application.Common.DTOs.Helper;
using WaffarXPartnerApi.Application.Common.DTOs.Shared.ExitClick;
using WaffarXPartnerApi.Application.Common.DTOs.Valu.SharedModels;
using WaffarXPartnerApi.Application.Common.DTOs.Valu.ValuRequestDto.GetFeaturedProductRequest;
using WaffarXPartnerApi.Application.Common.DTOs.Valu.ValuRequestDto.GetStoresRequest;
using WaffarXPartnerApi.Application.Common.DTOs.Valu.ValuRequestDto.ProductSearchRequest;
using WaffarXPartnerApi.Application.Common.DTOs.Valu.ValuRequestDto.StoreProductSearchRequest;
using WaffarXPartnerApi.Application.Common.DTOs.Valu.ValuResponseDto;
using WaffarXPartnerApi.Application.Common.DTOs.ValuRequestDto;
using WaffarXPartnerApi.Application.Common.DTOs.ValuResponseDto;
using WaffarXPartnerApi.Application.Common.Models.SharedModels;
using WaffarXPartnerApi.Application.ServiceImplementation.Shared;
using WaffarXPartnerApi.Application.ServiceInterface;
using WaffarXPartnerApi.Domain.Enums;
using WaffarXPartnerApi.Domain.Models.SharedModels;
using WaffarXPartnerApi.Domain.RepositoryInterface.EntityFrameworkRepositoryInterface;
using WaffarXPartnerApi.Domain.RepositoryInterface.EntityFrameworkRepositoryInterface.WaffarX;

namespace WaffarXPartnerApi.Application.ServiceImplementation;
public class ValuService : BaseService, IValuService
{
    private readonly IHttpService _httpService;
    private readonly IApiClientRepository _apiClientRepository;
    private readonly IAdvertiserRepository _advertiserRepository;

    public ValuService(IHttpService httpService, IHttpContextAccessor httpContextAccessor
        , IApiClientRepository apiClientRepository, IAdvertiserRepository advertiserRepository) : base(httpContextAccessor)
    {
        _httpService = httpService;
        _apiClientRepository = apiClientRepository;
        _advertiserRepository = advertiserRepository;   
    }

    public async Task<GenericResponseWithCount<List<BaseProductSearchResultDto>>> GetFeaturedProducts(GetFeaturedProductDto product)
    {
        try
        {
            var headers = new Dictionary<string, string>
            {
                ["Content-Type"] = "application/json"
            };
            GetFeaturedProductRequestDto requestObj = new GetFeaturedProductRequestDto
            {
                ClientApiId = ClientApiId,
                Count = product.PageSize,
                PageNumber = product.PageNumber,
                IsEnglish = IsEnglish,
            };
            // Make the POST request using our generic HTTP service
            var searchResults = await _httpService.PostAsync<GenericResponseWithCount<List<ProductSearchResponseModel>>>(
                AppSettings.ExternalApis.ValuUrl + "GetFeaturedProducts",
                requestObj,
                headers);
            if (searchResults.Status == StaticValues.Success && searchResults.Data.Any())
            {
                List<BaseProductSearchResultDto> products = new List<BaseProductSearchResultDto>();
                foreach (var item in searchResults.Data)
                {
                    products.Add(MapToBaseProduct(item));
                }

                return new GenericResponseWithCount<List<BaseProductSearchResultDto>>
                {
                    Status = StaticValues.Success,
                    Data = products,
                    TotalCount = searchResults.TotalCount,
                };
            }
            return new GenericResponseWithCount<List<BaseProductSearchResultDto>>()
            {
                Data = new List<BaseProductSearchResultDto>(),
                Status = StaticValues.Error,
                TotalCount = 0,
            };
        }
        catch(Exception)
        {
            throw;
        }
        
    }

    public async Task<GenericResponse<DetailedProductSearchResultDto>> GetProductDetails(string id)
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
                ProductId = id,
                ClientApiId = ClientApiId,
            };

            // Make the POST request using our generic HTTP service
            var searchResults = await _httpService.PostAsync<GenericResponse<ProductSearchResponseModel>>(
                AppSettings.ExternalApis.ValuUrl + "product",
                product,
                headers);
            if (searchResults.Status == StaticValues.Success && searchResults.Data != null)
            {
                return new GenericResponse<DetailedProductSearchResultDto>
                {
                    Data = MapToDetailProduct(searchResults.Data),
                    Status = StaticValues.Success
                };
            }

            return new GenericResponse<DetailedProductSearchResultDto>()
            {
                Data = new DetailedProductSearchResultDto(),
                Status = StaticValues.Error,
            };
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<GenericResponse<StoreDetailDto>> GetStoreDetails(Guid storeId)
    {
        try
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
                List<BaseProductSearchResultDto> products = new List<BaseProductSearchResultDto>();
                foreach (var item in searchResults.Data.StoreFeaturedProducts)
                {
                    products.Add(MapToBaseProduct(item));
                }
                return new GenericResponse<StoreDetailDto>
                {
                    Status = StaticValues.Success,
                    Data = new StoreDetailDto
                    {
                        StoreFeaturedProducts = products,
                        Store = new StoreResponseDto
                        {
                            Id = (Guid)(searchResults.Data?.Store.Id),
                            Logo = searchResults.Data?.Store.Logo,
                            Name = searchResults.Data?.Store.Name,
                            LogoPng = searchResults.Data?.Store.LogoPng,
                            ShoppingUrl = AppSettings.ExternalApis.ExitClickBaseUrl.Replace("{Partner}", "valu") + StaticValues.Store + searchResults.Data.Store.Id,
                        }
                    }
                };
            }
            return new GenericResponse<StoreDetailDto>()
            {
                Data = new StoreDetailDto(),
                Status = StaticValues.Error,
            };
        }
        catch(Exception)
        { throw; }
     
    }

    public async Task<GenericResponseWithCount<List<StoreResponseDto>>> GetStores(GetStoresRequestDto model)
    {
        try
        {
            var headers = new Dictionary<string, string>
            {
                ["Content-Type"] = "application/json"
            };
            GetStoresDto requestBody = new GetStoresDto
            {
                ClientApiId = ClientApiId,
                IsEnglish = IsEnglish,
                ItemCount = model.PageSize,
                PageNumber = model.PageNumber,

            };
            // Make the POST request using our generic HTTP service
            var searchResults = await _httpService.PostAsync<GenericResponseWithCount<List<StoreDto>>>(
                AppSettings.ExternalApis.ValuUrl + "GetStores",
                requestBody,
                headers);
            if (searchResults.Status == StaticValues.Success && searchResults.Data != null)
            {
                List<StoreResponseDto> stores = new List<StoreResponseDto>();
                foreach (var item in searchResults.Data)
                {
                    stores.Add(new StoreResponseDto
                    {
                        Id = (Guid)item.Id,
                        Logo = item.Logo,
                        LogoPng = item.LogoPng,
                        Name = item.Name,
                        ShoppingUrl = AppSettings.ExternalApis.ExitClickBaseUrl.Replace("{Partner}", "valu") + StaticValues.Store + item.Id
                    });
                }
                return new GenericResponseWithCount<List<StoreResponseDto>>
                {
                    Status = StaticValues.Success,
                    Data = stores,
                    TotalCount = searchResults.TotalCount,
                };
            }
            return new GenericResponseWithCount<List<StoreResponseDto>>
            {
                Data = new List<StoreResponseDto>(),
                Status = StaticValues.Error,
            };
        }
        catch(Exception)
        {
            throw;
        }
      
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
            ProductSearchDto requestBody = new ProductSearchDto
            {

                ClientApiId = ClientApiId.Value,
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
                    products.Add(MapToBaseProduct(product));
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

    public async Task<GenericResponse<string>> CreateExitClick(string section, Guid storeId, string productId = "", string userIdentifier = ""
        , string shoppingTripIdentifier = "", string variant = "")
    {
        GenericResponse<string> response = new GenericResponse<string>();
        try
        {
            // Get All Data Needed
            long userId = await _apiClientRepository.GetUserIdByClient(ClientApiId.ToString());
            int clientId = await _apiClientRepository.GetClientIdByGuid(ClientApiId.ToString());
            // Create ExitClick Request Data
            CreateExitClickRequestDto requestBody = RequestDataHelper.CreateExitClickRequestData((int)userId
                , (int)ExitClickSourcesEnum.Valu, IsEnglish, section, storeId, productId, userIdentifier, shoppingTripIdentifier, variant, clientId);

            var headers = new Dictionary<string, string>
            {
                ["Content-Type"] = "application/json"
            };

            // Make the POST request using our generic HTTP service
            var searchResults = await _httpService.PostAsync<GenericResponse<ExitClickResponseDto>>(
                AppSettings.ExternalApis.SharedApiUrl + "ExitClick/CreateExitClick",
                requestBody,
                headers);
            if (searchResults != null  && searchResults.Data != null && searchResults.Status == StaticValues.Success
                 && !string.IsNullOrEmpty(searchResults.Data.RedirectUrl))
            {
                response.Status = StaticValues.Success; 
                response.Data =  searchResults.Data.RedirectUrl;
                return response;
            }

            response.Status = StaticValues.Error;
            response.Data = "";
            response.Errors = new List<string>();   
            return response;
        }
        catch (Exception)
        {
            throw;
        }
    }
    private BaseProductSearchResultDto MapToBaseProduct(ProductSearchResponseModel model)
    {
        try
        {
            if (model == null)
                return null;
            BaseProductSearchResultDto res = new BaseProductSearchResultDto
            {
                Id = model.Id,
                Name = model.Name,
                MerchantName = model.AdvertiserName,
                Price = model.Price,
                Brand = model.Brand,
                Currency = model.Currency,
                PrimaryImg = model.PrimaryImg,
                VariantsImgs = model.VariantsImgs,
                Category = model.Category,
                ErrorImg = model.ErrorImg,
                OldPrice = model.OldPrice,
                OldPriceText = model.OldPriceText,
                PriceText = model.PriceText,
                Store = model.Store == null ? new StoreResponseDto() :
                new StoreResponseDto
                {
                    Id = (Guid)(model.Store?.Id),
                    Logo = model?.Store?.Logo,
                    Name = model?.Store?.Name,
                    LogoPng = model?.Store?.LogoPng,
                    ShoppingUrl = AppSettings.ExternalApis.ExitClickBaseUrl.Replace("{Partner}", "valu") + StaticValues.Store + model.Store.Id,
                    ShoppingUrlBase = AppSettings.ExternalApis.EClickAuthBaseUrl.Replace("{Partner}", "valu") + StaticValues.Store + model.Store.Id,
                },
                Offers = model?.Offers,
                ShoppingUrl = AppSettings.ExternalApis.ExitClickBaseUrl.Replace("{Partner}", "valu") + StaticValues.Product + model.Store.Id + "/" + model.Id,
                ShoppingUrlBase = AppSettings.ExternalApis.EClickAuthBaseUrl.Replace("{Partner}", "valu") + StaticValues.Product + model.Store.Id + "/" + model.Id
            };
            return res;

        }
        catch (Exception)
        {
            throw;
        }

    }
    private DetailedProductSearchResultDto MapToDetailProduct(ProductSearchResponseModel model)
    {
        try
        {
            if (model == null)
                return null;
            DetailedProductSearchResultDto res = new DetailedProductSearchResultDto
            {
                Id = model.Id,
                Name = model.Name,
                MerchantName = model.AdvertiserName,
                Price = model.Price,
                Brand = model.Brand,
                Currency = model.Currency,
                PrimaryImg = model.PrimaryImg,
                VariantsImgs = model.VariantsImgs,
                Category = model.Category,
                ErrorImg = model.ErrorImg,
                OldPrice = model.OldPrice,
                OldPriceText = model.OldPriceText,
                PriceText = model.PriceText,
                Store = model.Store == null ? new StoreResponseDto() :
                new StoreResponseDto
                {
                    Id = (Guid)(model.Store?.Id),
                    Logo = model?.Store?.Logo,
                    Name = model?.Store?.Name,
                    LogoPng = model?.Store?.LogoPng,
                    ShoppingUrl = AppSettings.ExternalApis.ExitClickBaseUrl.Replace("{Partner}", "valu") + StaticValues.Store + model.Store.Id, 
                    ShoppingUrlBase = AppSettings.ExternalApis.EClickAuthBaseUrl.Replace("{Partner}", "valu") + StaticValues.Store + model.Store.Id 

                },
                Offers = model?.Offers,
                Description = model.Description,
                DiscountedText = model.DiscountedText,
                Discounted = model.Discounted,
                Feature = model.Features,
                SKU = model.SKU,
                Warranty = model.Warranty,
                Specification = model.Specification?.ToDictionary(k => k.Key, v => (object)v),
                Options = (model.options as IEnumerable<OptionDto>)?.Select(o => new OptionDto
                {
                    Name = o.Name,
                    Id = o.Id,
                    ProductId = o.ProductId,
                    Position = o.Position,
                    Values = o.Values
                }).ToList(),
                PriceVariants = model.price_variants?.Select(v => new PriceVariantDto
                {
                    Price = v.price,
                    Title = v.title,
                    Available = v.available,
                    Options = v.options,
                    PriceText = v.PriceText,
                    DiscountedText = v.DiscountedText,
                    Discounted = v.Discounted,
                    OldPrice = v.old_price,
                    OldPriceText = v.OldPriceText,    
                    ShoppingURL = AppSettings.ExternalApis.ExitClickBaseUrl.Replace("{Partner}", "valu") + StaticValues.Product + model.Store.Id + "/" + model.Id + StaticValues.Variant + v.variant_id,
                    ShoppingUrlBase = AppSettings.ExternalApis.EClickAuthBaseUrl.Replace("{Partner}", "valu") + StaticValues.Product + model.Store.Id + "/" + model.Id
                }).ToList(),
                ShoppingUrl = AppSettings.ExternalApis.ExitClickBaseUrl.Replace("{Partner}", "valu") + StaticValues.Product + model.Store.Id + "/" + model.Id,
                ShoppingUrlBase = AppSettings.ExternalApis.EClickAuthBaseUrl.Replace("{Partner}", "valu") + StaticValues.Product + model.Store.Id + "/" + model.Id

            };
            return res;
        
        }
        catch (Exception)
        {
            throw;
        }
    }
    public async Task<GenericResponse<ProductSearchResultWithFiltersDto>> StoreSearchProduct(StoreProductSearchRequestDto storeProductSearch)
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
           
            List<Guid> guids = new List<Guid>() { storeProductSearch.StoreId };
            List<int> storeIds = await _advertiserRepository.GetStoreIds(guids);

            filterModel = new SearchFilterModel
            {
                Brands = "",
                Stores = storeIds,
                MinPrice = storeProductSearch?.MinPrice,
                MaxPrice = storeProductSearch?.MaxPrice,
                Category = storeProductSearch?.Category,    
            };

            ProductSearchDto requestBody = new ProductSearchDto
            {
                ClientApiId = ClientApiId.Value,
                IsEnglish = IsEnglish,
                PageNumber = storeProductSearch.PageNumber,
                ItemCount = storeProductSearch.PageSize,
                SearchText = "",
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
                    products.Add(MapToBaseProduct(product));
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
                Data = new ProductSearchResultWithFiltersDto(),
                Status = StaticValues.Error
            };
        }
        catch (Exception)
        {
            throw;
        }
    }
    public async Task<GenericResponse<StoreCategoriesDto>> GetStoreCategories(Guid storeId)
    {
        try
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
            var stroreCategoriesResults = await _httpService.PostAsync<GenericResponse<StoreCategoriesModel>>(
                AppSettings.ExternalApis.ValuUrl + "GetStoreCategories",
                store,
                headers);
            if (stroreCategoriesResults.Status == StaticValues.Success && stroreCategoriesResults.Data != null
                && stroreCategoriesResults.Data.StoreId > 0 
                && (stroreCategoriesResults.Data.EnCategoriesList?.Count > 0 || stroreCategoriesResults.Data.ArCategoriesList?.Count > 0))
            {
                return new GenericResponse<StoreCategoriesDto>
                {
                    Status = StaticValues.Success,
                    Data = new StoreCategoriesDto
                    {
                        CategoriesEn = stroreCategoriesResults.Data.EnCategoriesList,
                        CategoriesAr = stroreCategoriesResults.Data.ArCategoriesList,
                    }
                };
            }
            return new GenericResponse<StoreCategoriesDto>()
            {
                Data = new StoreCategoriesDto(),
                Status = StaticValues.Error,
            };
        }
        catch (Exception)
        {
            throw; 
        }
    }

}
