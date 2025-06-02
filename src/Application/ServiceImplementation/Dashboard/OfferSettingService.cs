using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using MongoDB.Bson;
using MongoDB.Driver;
using WaffarXPartnerApi.Application.Common.DTOs.Dashboard.Offers;
using WaffarXPartnerApi.Application.Common.DTOs.Dashboard.Offers.OfferLookUp;
using WaffarXPartnerApi.Application.Common.DTOs.Dashboard.Offers.OfferSetting;
using WaffarXPartnerApi.Application.Common.DTOs.Dashboard.Offers.OfferType;
using WaffarXPartnerApi.Application.Common.DTOs.Valu.SharedModels;
using WaffarXPartnerApi.Application.Common.DTOs.Valu.ValuRequestDto;
using WaffarXPartnerApi.Application.Common.DTOs.ValuRequestDto;
using WaffarXPartnerApi.Application.Common.DTOs.ValuResponseDto;
using WaffarXPartnerApi.Application.Common.Models.SharedModels;
using WaffarXPartnerApi.Application.Helper;
using WaffarXPartnerApi.Application.ServiceImplementation.Shared;
using WaffarXPartnerApi.Application.ServiceInterface.Dashboard;
using WaffarXPartnerApi.Application.ServiceInterface.Shared;
using WaffarXPartnerApi.Domain.Models.PartnerMongoModels;
using WaffarXPartnerApi.Domain.Models.SharedModels;
using WaffarXPartnerApi.Domain.RepositoryInterface.EntityFrameworkRepositoryInterface;
using WaffarXPartnerApi.Domain.RepositoryInterface.MongoRepositoryInterface;

namespace WaffarXPartnerApi.Application.ServiceImplementation.Dashboard;
public class OfferSettingService : JWTUserBaseService, IOfferSettingService
{
    private readonly IHttpService _httpService;

    private readonly IApiClientRepository _apiClientRepository;
    private readonly IPartnerRepository _partnerRepository;
    private readonly IMapper _mapper;

    public OfferSettingService(IHttpContextAccessor httpContextAccessor, IHttpService httpService
        , IApiClientRepository apiClientRepository, IPartnerRepository partnerRepository
        , IMapper mapper) : base(httpContextAccessor)
    {
        _httpService = httpService;
        _apiClientRepository = apiClientRepository;
        _partnerRepository = partnerRepository;
        _mapper = mapper;   
    }

    #region Offer Setting
    public async Task<GenericResponse<bool>> AddOrUpdateOffer(OfferSettingRequestDto model)
    {
        List<int> ProductStores = new List<int>();
        try
        {
            List<int> storesList =  await _partnerRepository.GetStoreIdsByStoreGuids(model.StoreIds);
            if (model.IsProductLevel == true && model.ProductIds?.Count > 0)
            { 
                ProductStores = await GetStoreIdsByProducts(model.ProductIds);
            }
            OfferSettingModel offerSettingModel = new OfferSettingModel
            {
                ClientApiId = ClientApiId,
                OfferLookUpId = model.OfferLookUpId,
                StartDate = model.StartDate,
                EndDate = model.EndDate,
                IsProductLevel = model.IsProductLevel,
                IsStoreLevel = model.IsStoreLevel,
                UserId = UserIdInt,
                Id = model.Id,
                ProductIds = model.ProductIds,
                StoreIds = storesList,
                OfferTypeId = model.OfferTypeId,
                IsFixed = model.IsFixed,
                Amount = model.Amount,
                ProductStoreIds = ProductStores 
            };
            var result = await _partnerRepository.AddUpdateOfferSetting(offerSettingModel);
            return new GenericResponse<bool>
            {
                Data = true,
                Status = StaticValues.Success
            };
        }
        catch (Exception)
        {
            throw;
        }
    }
    public async Task<GenericResponse<List<OfferResponseDto>>> GetOffers()
    {
        try
        {
            List<OfferSettingListingModel> offers = new List<OfferSettingListingModel>();
            offers = await _partnerRepository.GetOfferSettingListing(ClientApiId);
            if (!offers.Any())
            {
                return new GenericResponse<List<OfferResponseDto>>
                {
                    Data = new List<OfferResponseDto>(),
                    Status = StaticValues.Success
                };
            }
            List<OfferResponseDto> response = new List<OfferResponseDto>();
            foreach (var item in offers)
            {
                response.Add(new OfferResponseDto
                {
                    EndDate = item.EndDate,
                    IsProductLevel = item.IsProductLevel,
                    IsStoreLevel = item.IsStoreLevel,
                    OfferId = item.OfferId,
                    OfferName = item.OfferName,
                    StartDate = item.StartDate,
                    Amount = item.Amount,
                    IsFixed = item.IsFixed
                });

            }
            return new GenericResponse<List<OfferResponseDto>>
            {
                Data = response,
                Status = StaticValues.Success
            };
        }
        catch (Exception)
        {
            throw;
        }
    }
    public async Task<GenericResponse<OfferDetailResponseDto>> GetOfferDetails(string id)
    {
        try
        {
            var offerSetting = await _partnerRepository.GetOfferSetting(ClientApiId, id);
            if (offerSetting == null)
            {
                return new GenericResponse<OfferDetailResponseDto>
                {
                    Data = new OfferDetailResponseDto
                    {
                        Products = new List<BaseProductSearchResultDto>(),
                        Stores = new List<StoreDto>()
                    },
                    Status = StaticValues.Success
                };
            }
            var offerType = await _partnerRepository.GetOfferType(ClientApiId, offerSetting.OfferTypeId);
            OfferDetailResponseDto response = new OfferDetailResponseDto();
            response.StartDate = offerSetting.StartDate;
            response.EndDate = offerSetting.EndDate;
            response.IsProductLevel = offerSetting.IsProductLevel;
            response.IsStoreLevel = offerSetting.IsStoreLevel;
            response.OfferLookupId = offerSetting.OfferLookUpId.ToString();
            response.OfferTypeId = offerSetting.OfferTypeId.ToString();
            response.OfferTypeName = offerType?.NameEn ?? "";
            response.Amount = offerSetting.Amount;
            response.IsFixed = offerSetting.IsFixed;

            if (offerSetting.IsProductLevel)
            {
                var headers = new Dictionary<string, string>
                {
                    ["Content-Type"] = "application/json"
                };
                var clientGuid = await _apiClientRepository.GetClientGuidById(ClientApiId);

                GetProductDetailsRequestDto requestBody = new GetProductDetailsRequestDto
                {
                    ClientApiId = clientGuid,
                    IsEnglish = IsEnglish,
                    Products = offerSetting.ProductIds?.Select(id => id.ToString()).ToList()
                };
                // Make the POST request using our generic HTTP service
                var searchResults = await _httpService.PostAsync<GenericResponse<ValuSearchResponseDto>>(
                    AppSettings.ExternalApis.ValuUrl + "GetProductDetails",
                    requestBody,
                    headers);
                if (searchResults.Status == StaticValues.Success && searchResults.Data != null)
                {
                    List<BaseProductSearchResultDto> products = new List<BaseProductSearchResultDto>();
                    foreach (var product in searchResults.Data.Products)
                    {
                        products.Add(ProductMappingHelper.MapToBaseProduct(product));
                    }
                    response.Products = products;
                }
            }
            if (offerSetting.IsStoreLevel)
            {
                List<StoreModel> storesDetailsToGet = await _partnerRepository.GetStoreDataByStoreIds(offerSetting.StoreIds);
                response.Stores = new List<StoreDto>();
                if (storesDetailsToGet.Any())
                {
                    foreach (var store in storesDetailsToGet)
                    {
                        response.Stores.Add(new StoreDto
                        {
                            Id = store.Id,
                            Name = store.Name,
                            Logo = store.Logo,
                            LogoPng = store.LogoPng
                        });
                    }
                }

            }
            return new GenericResponse<OfferDetailResponseDto>
            {
                Data = response,
                Status = StaticValues.Success,
            };
        }
        catch (Exception)
        {
            throw;
        }
    }
    #endregion

    #region Offer Lookup
    public async Task<GenericResponse<bool>> AddOrUpdateOfferLookUp(OfferLookUpRequestDto model)
    {
        try
        {
            OfferLookUpModel offerLookUp = new OfferLookUpModel
            {
                ClientApiId = ClientApiId,
                Id = model.Id,
                NameAr = model.NameAr,
                NameEn = model.NameEn,
                UserId = UserIdInt,
                DescriptionAr = model.DescriptionAr,
                DescriptionEn = model.DescriptionEn,

            };
            await _partnerRepository.AddOfferLookUp(offerLookUp);
            return new GenericResponse<bool>
            {
                Data = true,
                Message = StaticValues.Success
            };
        }
        catch (Exception)
        {
            throw;
        }

    }
    public async Task<GenericResponse<List<OfferLookUpResponsetDto>>> GetOffersLookup()
    {
        try
        {
            var offerLookUp = await _partnerRepository.GetOfferLookUp(ClientApiId);
            var response = new List<OfferLookUpResponsetDto>();
            foreach (var offer in offerLookUp)
            {
                response.Add(new OfferLookUpResponsetDto
                {
                    Id = offer.Id,
                    NameEn = offer.NameEn,
                    NameAr = offer.NameAr,
                    DescriptionAr = offer.DescriptionAr,
                    DescriptionEn = offer.DescriptionEn,    
                });
            }
            return new GenericResponse<List<OfferLookUpResponsetDto>>
            {
                Data = response,
                Message = StaticValues.Success
            };

        }
        catch (Exception)
        {
            throw;
        }
    }

    #endregion

    #region Offer Types

    public async Task<GenericResponse<bool>> AddOrUpdateOfferType(OfferTypeRequestDto model)
    {
        try
        {
            OfferTypeModel offerType = new OfferTypeModel
            {
                ClientApiId = ClientApiId,
                Id = model.Id,
                NameAr = model.NameAr,
                NameEn = model.NameEn,
                UserId = UserIdInt,
            };
            await _partnerRepository.AddOfferType(offerType);
            return new GenericResponse<bool>
            {
                Data = true,
                Status = StaticValues.Success
            };
        }
        catch (Exception)
        {
            throw;
        }

    }
    public async Task<GenericResponse<List<OfferTypeResponsetDto>>> GetOfferTypes()
    {
        List<OfferTypeResponsetDto> response = new List<OfferTypeResponsetDto>();
        try
        {
            var offerTypes = await _partnerRepository.GetOfferTypes(ClientApiId);
            if (offerTypes?.Count > 0)
            {
                response = _mapper.Map<List<OfferTypeResponsetDto>>(offerTypes);
            }
            return new GenericResponse<List<OfferTypeResponsetDto>>
            {
                Data = response,
                Status = StaticValues.Success
            };

        }
        catch (Exception)
        {
            throw;
        }
    }
    #endregion

    private async Task<List<int>> GetStoreIdsByProducts(List<string> productIds)
    {
        var headers = new Dictionary<string, string>
        {
            ["Content-Type"] = "application/json"
        };
        GetStoreIdsByProductsDto product = new GetStoreIdsByProductsDto
        {
            ProductIds = productIds,
        };

        // Make the POST request using our generic HTTP service
        var searchResults = await _httpService.PostAsync<GenericResponse<List<int>>>(
            AppSettings.ExternalApis.ValuUrl + "GetStoresIds", product, headers);

        if (searchResults.Status == StaticValues.Success && searchResults.Data != null)
        {
            return searchResults.Data;
        }
        return new List<int>();
    }

}
