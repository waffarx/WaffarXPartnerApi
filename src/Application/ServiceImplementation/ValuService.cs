﻿using Microsoft.AspNetCore.Http;
using WaffarXPartnerApi.Application.Common.DTOs.Helper;
using WaffarXPartnerApi.Application.Common.DTOs.Shared.ExitClick;
using WaffarXPartnerApi.Application.Common.DTOs.Valu.SharedModels;
using WaffarXPartnerApi.Application.Common.DTOs.Valu.ValuRequestDto.GetFeaturedByStoreRequest;
using WaffarXPartnerApi.Application.Common.DTOs.Valu.ValuRequestDto.GetFeaturedProductRequest;
using WaffarXPartnerApi.Application.Common.DTOs.Valu.ValuRequestDto.GetProductsOffersRequest;
using WaffarXPartnerApi.Application.Common.DTOs.Valu.ValuRequestDto.GetStoreBrands;
using WaffarXPartnerApi.Application.Common.DTOs.Valu.ValuRequestDto.GetStoresRequest;
using WaffarXPartnerApi.Application.Common.DTOs.Valu.ValuRequestDto.GetStoresWithProductsRequest;
using WaffarXPartnerApi.Application.Common.DTOs.Valu.ValuRequestDto.ProductSearchRequest;
using WaffarXPartnerApi.Application.Common.DTOs.Valu.ValuRequestDto.StoreProductSearchRequest;
using WaffarXPartnerApi.Application.Common.DTOs.Valu.ValuRequestDto.StoreSearchWithFiltersRequest;
using WaffarXPartnerApi.Application.Common.DTOs.Valu.ValuResponseDto;
using WaffarXPartnerApi.Application.Common.DTOs.Valu.ValuResponseDto.StoreDetails;
using WaffarXPartnerApi.Application.Common.DTOs.ValuRequestDto;
using WaffarXPartnerApi.Application.Common.DTOs.ValuResponseDto;
using WaffarXPartnerApi.Application.Common.Models.SharedModels;
using WaffarXPartnerApi.Application.Helper;
using WaffarXPartnerApi.Application.ServiceImplementation.Shared;
using WaffarXPartnerApi.Application.ServiceInterface;
using WaffarXPartnerApi.Application.ServiceInterface.Shared;
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

    public async Task<GenericResponseWithCount<List<BaseProductSearchResultDto>>> GetFeaturedProducts(GetFeaturedProductByStoreDto product)
    {
        try
        {
            var headers = new Dictionary<string, string>
            {
                ["Content-Type"] = "application/json"
            };
            Guid? storeId = null;
            if (product.StoreId != null && !string.IsNullOrEmpty(product.StoreId))
            {
                storeId = new Guid(product.StoreId);
            }
            string Url = (storeId != null && storeId.HasValue) ? "GetFeaturedProductsByStore" : "GetFeaturedProducts";
            GetFeaturedProductByStoreRequestDto requestObj = new GetFeaturedProductByStoreRequestDto
            {
                ClientApiId = ClientApiId,
                Count = product.PageSize,
                PageNumber = product.PageNumber,
                IsEnglish = IsEnglish,
                StoreId = storeId,  
            };
            // Make the POST request using our generic HTTP service
            var searchResults = await _httpService.PostAsync<GenericResponseWithCount<List<ProductSearchResponseModel>>>(
                AppSettings.ExternalApis.ValuUrl + Url, requestObj, headers);

            if (searchResults.Status == StaticValues.Success && searchResults.Data.Any())
            {
                List<BaseProductSearchResultDto> products = new List<BaseProductSearchResultDto>();
                foreach (var item in searchResults.Data)
                {
                    products.Add(ProductMappingHelper.MapToBaseProduct(item));
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
        catch (Exception)
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
                    Data = ProductMappingHelper.MapToDetailProduct(searchResults.Data),
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
                AppSettings.ExternalApis.ValuUrl + "GetStoreDetails", store, headers);
            if (searchResults.Status == StaticValues.Success && searchResults.Data != null)
            {
                List<BaseProductSearchResultDto> products = new List<BaseProductSearchResultDto>();
                foreach (var item in searchResults.Data.StoreFeaturedProducts)
                {
                    products.Add(ProductMappingHelper.MapToBaseProduct(item));
                }
                return new GenericResponse<StoreDetailDto>
                {
                    Status = StaticValues.Success,
                    Data = new StoreDetailDto
                    {
                        StoreFeaturedProducts = products,
                        Store = new StoreWithOffersResponseDto
                        {
                            Id = (Guid)(searchResults.Data?.Store.Id),
                            Logo = searchResults.Data?.Store.Logo,
                            Name = searchResults.Data?.Store.Name,
                            LogoPng = searchResults.Data?.Store.LogoPng,
                            ShoppingUrl = AppSettings.ExternalApis.ExitClickBaseUrl.Replace("{Partner}", "valu") + StaticValues.Store + searchResults.Data.Store.Id,
                            ShoppingUrlBase = AppSettings.ExternalApis.EClickAuthBaseUrl.Replace("{Partner}", "valu") + StaticValues.Store + searchResults.Data.Store.Id,
                            BackgroundColor = searchResults.Data?.Store.BackgroundColor,
                            Offers = searchResults.Data?.Store.Offers ?? new List<OfferDto>(),
                        },
                        
                    }
                };
            }
            return new GenericResponse<StoreDetailDto>()
            {
                Data = new StoreDetailDto(),
                Status = StaticValues.Error,
            };
        }
        catch (Exception)
        { 
            throw; 
        }

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
                        Id = item.Id,
                        Logo = item.Logo,
                        LogoPng = item.LogoPng,
                        Name = item.Name,
                        ShoppingUrl = AppSettings.ExternalApis.ExitClickBaseUrl.Replace("{Partner}", "valu") + StaticValues.Store + item.Id,
                        ShoppingUrlBase = AppSettings.ExternalApis.EClickAuthBaseUrl.Replace("{Partner}", "valu") + StaticValues.Store + item.Id
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
        catch (Exception)
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
                    OfferId = productSearch?.Filter?.OfferId,
                    Discounted = productSearch?.Filter?.Discounted ?? false,
                };
            }
            ProductSearchDto requestBody = new ProductSearchDto
            {

                ClientApiId = ClientApiId.Value,
                IsEnglish = IsEnglish,
                PageNumber = productSearch.PageNumber,
                ItemCount = productSearch.PageSize,
                SearchText = productSearch.SearchText,
                SortByPriceDsc = productSearch.SortByPriceDsc,
                Filter = filterModel
            };

            // Make the POST request using our generic HTTP service
            var searchResults = await _httpService.PostAsync<GenericResponse<ValuSearchResponseDto>>(AppSettings.ExternalApis.ValuUrl + "search", requestBody, headers);
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
                AppSettings.ExternalApis.SharedApiUrl + "ExitClick/CreateExitClick", requestBody, headers);

            if (searchResults != null && searchResults.Data != null && searchResults.Status == StaticValues.Success
                 && !string.IsNullOrEmpty(searchResults.Data.RedirectUrl))
            {
                response.Status = StaticValues.Success;
                response.Data = searchResults.Data.RedirectUrl;
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
                Stores = storeIds,
                MinPrice = storeProductSearch?.MinPrice,
                MaxPrice = storeProductSearch?.MaxPrice,
                Category = storeProductSearch?.Category,
                Brands = storeProductSearch?.Brand ?? "",
                Discounted = storeProductSearch?.Discounted ?? false,
            };

            ProductSearchDto requestBody = new ProductSearchDto
            {
                ClientApiId = ClientApiId.Value,
                IsEnglish = IsEnglish,
                PageNumber = storeProductSearch.PageNumber,
                ItemCount = storeProductSearch.PageSize,
                SearchText = "",
                Filter = filterModel,
                SortByPriceDsc = storeProductSearch.SortByPriceDsc,
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
                        Filters = null
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
    public async Task<GenericResponseWithCount<List<StoreWithOffersResponseDto>>> GetStoresWithOffers(GetStoresRequestDto model)
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
            var searchResults = await _httpService.PostAsync<GenericResponseWithCount<List<StoreDetailsWithOffersDto>>>(
                AppSettings.ExternalApis.ValuUrl + "GetStoresWithOffers",
                requestBody,
                headers);
            if (searchResults.Status == StaticValues.Success && searchResults.Data != null)
            {
                List<StoreWithOffersResponseDto> stores = new List<StoreWithOffersResponseDto>();
                foreach (var item in searchResults.Data)
                {
                    stores.Add(new StoreWithOffersResponseDto
                    {
                        Id = item.Id,
                        Logo = item.Logo,
                        LogoPng = item.LogoPng,
                        Name = item.Name,
                        ShoppingUrl = AppSettings.ExternalApis.ExitClickBaseUrl.Replace("{Partner}", "valu") + StaticValues.Store + item.Id,
                        ShoppingUrlBase = AppSettings.ExternalApis.EClickAuthBaseUrl.Replace("{Partner}", "valu") + StaticValues.Store + item.Id,
                        BackgroundColor = item.BackgroundColor,
                        Offers = item.Offers ?? new List<OfferDto>(),
                    });
                }
                return new GenericResponseWithCount<List<StoreWithOffersResponseDto>>
                {
                    Status = StaticValues.Success,
                    Data = stores,
                    TotalCount = searchResults.TotalCount,
                };
            }
            return new GenericResponseWithCount<List<StoreWithOffersResponseDto>>
            {
                Data = new List<StoreWithOffersResponseDto>(),
                Status = StaticValues.Error,
            };
        }
        catch (Exception)
        {
            throw;
        }

    }
    public async Task<GenericResponseWithCount<List<StoreWithProductsResponseDto>>> GetStoresWithProducts(GetStoresWithProductsDto model)
    {
        try
        {
            var headers = new Dictionary<string, string>
            {
                ["Content-Type"] = "application/json"
            };
            GetStoresWithProductsRequestDto requestBody = new GetStoresWithProductsRequestDto
            {
                ClientApiId = ClientApiId,
                IsEnglish = IsEnglish,
                ItemCount = model.PageSize,
                PageNumber = model.PageNumber,
                ProductLimit = model.ProductsCount

            };
            // Make the POST request using our generic HTTP service
            var searchResults = await _httpService.PostAsync<GenericResponseWithCount<List<StoresWithOffersResponseModel>>>(
                AppSettings.ExternalApis.ValuUrl + "GetStoresWithProducts", requestBody,headers);

            if (searchResults.Status == StaticValues.Success && searchResults.Data != null)
            {
                List<StoreWithProductsResponseDto> stores = new List<StoreWithProductsResponseDto>();
                List<BaseProductSearchResultDto> products = new List<BaseProductSearchResultDto>();
              
                foreach (var item in searchResults.Data)
                {
                    products = new List<BaseProductSearchResultDto>();
                    foreach (var prod in item.StoreFeaturedProducts)
                    {
                        products.Add(ProductMappingHelper.MapToBaseProduct(prod));
                    }
                    stores.Add(new StoreWithProductsResponseDto
                    {
                        Id = item.Id,
                        Logo = item.Logo,
                        LogoPng = item.LogoPng,
                        Name = item.Name,
                        ShoppingUrl = AppSettings.ExternalApis.ExitClickBaseUrl.Replace("{Partner}", "valu") + StaticValues.Store + item.Id,
                        ShoppingUrlBase = AppSettings.ExternalApis.EClickAuthBaseUrl.Replace("{Partner}", "valu") + StaticValues.Store + item.Id,
                        BackgroundColor = !string.IsNullOrEmpty(item.BackgroundColor)  ? item.BackgroundColor :  "",
                        Offers = item.Offers ?? new List<OfferDto>(),
                        Products = products ?? new List<BaseProductSearchResultDto>()
                    });
                }
                return new GenericResponseWithCount<List<StoreWithProductsResponseDto>>
                {
                    Status = StaticValues.Success,
                    Data = stores,
                    TotalCount = searchResults.TotalCount,
                };
            }
            return new GenericResponseWithCount<List<StoreWithProductsResponseDto>>
            {
                Data = new List<StoreWithProductsResponseDto>(),
                Status = StaticValues.Error,
            };
        }
        catch (Exception)
        {
            throw;
        }

    }
    public async Task<GenericResponseWithCount<List<BaseProductSearchResultDto>>> GetProductsByOffers(GetProductsByOffersDto offersDto)
    {
        try
        {
            var headers = new Dictionary<string, string>
            {
                ["Content-Type"] = "application/json"
            };
            string Url = "GetProductsByOffers";
            GetProductsByOffersRequestDto requestObj = new GetProductsByOffersRequestDto
            {
                ClientApiId = ClientApiId,
                Count = offersDto.PageSize,
                PageNumber = offersDto.PageNumber,
                IsEnglish = IsEnglish
            };
            // Make the POST request using our generic HTTP service
            var searchResults = await _httpService.PostAsync<GenericResponseWithCount<List<ProductSearchResponseModel>>>(AppSettings.ExternalApis.ValuUrl + Url, requestObj, headers);
            if (searchResults.Status == StaticValues.Success && searchResults.Data.Any())
            {
                List<BaseProductSearchResultDto> products = new List<BaseProductSearchResultDto>();
                foreach (var item in searchResults.Data)
                {
                    products.Add(ProductMappingHelper.MapToBaseProduct(item));
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
        catch (Exception)
        {
            throw;
        }

    }
    public async Task<GenericResponseWithCount<List<OfferStoresResponseDto>>> GetStoresByActiveOffers(GetProductsByOffersDto model)
    {
        try
        {
            var headers = new Dictionary<string, string>
            {
                ["Content-Type"] = "application/json"
            };
            GetProductsByOffersRequestDto requestBody = new GetProductsByOffersRequestDto
            {
                ClientApiId = ClientApiId,
                IsEnglish = IsEnglish,
                Count = model.PageSize,
                PageNumber = model.PageNumber,

            };
            // Make the POST request using our generic HTTP service
            var searchResults = await _httpService.PostAsync<GenericResponseWithCount<List<OfferStoresResponseDto>>>(
                AppSettings.ExternalApis.ValuUrl + "GetStoresByOffers",
                requestBody,
                headers);
            if (searchResults.Status == StaticValues.Success && searchResults.Data != null)
            {
                List<OfferStoresResponseDto> offers = new List<OfferStoresResponseDto>();
                foreach (var item in searchResults.Data)
                {
                    offers.Add(new OfferStoresResponseDto
                    {
                        Id = item.Id.ToString(),
                        Name = item.Name,
                        Description = item.Description,
                        TypeName = item.TypeName,
                        IsRewardOffer = item.IsRewardOffer,
                        IsProductLevel = item.IsProductLevel,
                        IsStoreLevel = item.IsStoreLevel,
                        Stores = item.Stores.Select(s => new StoreResponseDto
                        {
                            Id = s.Id,
                            Logo = s.Logo,
                            LogoPng = s.LogoPng,
                            Name = s.Name,
                            ShoppingUrl = AppSettings.ExternalApis.ExitClickBaseUrl.Replace("{Partner}", "valu") + StaticValues.Store + s.Id,
                            ShoppingUrlBase = AppSettings.ExternalApis.EClickAuthBaseUrl.Replace("{Partner}", "valu") + StaticValues.Store + s.Id
                        }).ToList(),
                    });

                }
                return new GenericResponseWithCount<List<OfferStoresResponseDto>>
                {
                    Status = StaticValues.Success,
                    Data = offers,
                    TotalCount = searchResults.TotalCount,
                };
            }
            return new GenericResponseWithCount<List<OfferStoresResponseDto>>
            {
                Data = new List<OfferStoresResponseDto>(),
                Status = StaticValues.Error,
            };
        }
        catch (Exception)
        {
            throw;
        }

    }
    public async Task<GenericResponse<List<string>>> SearchBrandsByStore(GetStoreBrandsDto model)
    {
        try
        {
            var headers = new Dictionary<string, string>
            {
                ["Content-Type"] = "application/json"
            };
            GetStoreBrandsRequestDto requestBody = new GetStoreBrandsRequestDto
            {
                ClientApiId = ClientApiId,
                IsEnglish = IsEnglish,
                SearchText = model.SearchText, 
                StoreId = new Guid(model.StoreId)
            };
            // Make the POST request using our generic HTTP service
            var searchResults = await _httpService.PostAsync<GenericResponseWithCount<List<string>>>(
                AppSettings.ExternalApis.ValuUrl + "GetStoresBrands", requestBody,headers);

            if (searchResults.Status == StaticValues.Success && searchResults.Data?.Count > 0)
            {
                return new GenericResponse<List<string>>
                {
                    Status = StaticValues.Success,
                    Data = searchResults.Data,
                };
            }
            return new GenericResponse<List<string>>
            {
                Status = StaticValues.Error,
                Data = searchResults.Data,
                Errors = new List<string> { "No brands found for the given store." }    
            };
        }
        catch (Exception)
        {
            throw;
        }

    }
    public async Task<GenericResponse<StoreSearchResponseWithFiltersDto>> SearchByStoreWithFilter(StoreSearchWithFiltersDto searchByStore)
    {
        try
        {
            StoreSearchResponseWithFiltersDto response = new StoreSearchResponseWithFiltersDto();

            // Set up any headers you need
            var headers = new Dictionary<string, string>
            {
                ["Content-Type"] = "application/json"
            };
            ValuStoreSearchFilterDto filterModel = null;

            List<Guid> guids = new List<Guid>() { searchByStore.StoreId };
            List<int> storeIds = await _advertiserRepository.GetStoreIds(guids);

            filterModel = new ValuStoreSearchFilterDto
            {
                MinPrice = searchByStore.Filter?.MinPrice,
                MaxPrice = searchByStore.Filter?.MaxPrice,
                Category = searchByStore.Filter?.Category,
                Brands = searchByStore.Filter?.Brands ?? "",
                Discounted = searchByStore.Filter?.Discounted ?? false,
            };

            ValuStoreSearchQueryDto requestBody = new ValuStoreSearchQueryDto
            {
                StoreId = storeIds.FirstOrDefault(),
                ClientApiId = ClientApiId.Value,
                IsEnglish = IsEnglish,
                PageNumber = searchByStore.PageNumber,
                ItemCount = searchByStore.PageSize,
                Filter = filterModel,
                SortByPriceDsc = searchByStore.SortByPriceDsc,
            };

            // Make the POST request using our generic HTTP service
            var searchResults = await _httpService.PostAsync<GenericResponse<StoreSearchResultWithFiltersDto>>(
                AppSettings.ExternalApis.ValuUrl + "SearchByStore",
                requestBody,
                headers);
            if (searchResults.Data != null && searchResults.Data.Products.Any())
            {
                List<BaseProductSearchResultDto> products = new List<BaseProductSearchResultDto>();

                foreach (var product in searchResults.Data.Products)
                {
                    products.Add(ProductMappingHelper.MapToBaseProduct(product));
                }

                return new GenericResponse<StoreSearchResponseWithFiltersDto>
                {
                    Status = StaticValues.Success,
                    Data = new StoreSearchResponseWithFiltersDto
                    {
                        Products = products,
                        Filters = new StoreSearchFilterDto
                        {
                            Brands = searchResults.Data.Filters?.Brands,
                            Categories = searchResults.Data.Filters?.Categories,
                            MinPrice = searchResults.Data.Filters?.MinPrice,
                            MaxPrice = searchResults.Data.Filters?.MaxPrice,
                            Offers = searchResults.Data.Filters?.Offers,
                        }
                    }
                };

            }
            return new GenericResponse<StoreSearchResponseWithFiltersDto>
            {
                Data = new StoreSearchResponseWithFiltersDto(),
                Status = StaticValues.Error
            };
        }
        catch (Exception)
        {
            throw;
        }
    }
}
