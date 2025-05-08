using Microsoft.AspNetCore.Http;
using MongoDB.Bson;
using MongoDB.Driver;
using WaffarXPartnerApi.Application.Common.DTOs.Dashboard.Offers;
using WaffarXPartnerApi.Application.Common.DTOs.Valu.SharedModels;
using WaffarXPartnerApi.Application.Common.DTOs.Valu.ValuRequestDto;
using WaffarXPartnerApi.Application.Common.DTOs.ValuResponseDto;
using WaffarXPartnerApi.Application.Common.Models.SharedModels;
using WaffarXPartnerApi.Application.Helper;
using WaffarXPartnerApi.Application.ServiceImplementation.Shared;
using WaffarXPartnerApi.Application.ServiceInterface;
using WaffarXPartnerApi.Application.ServiceInterface.Dashboard;
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

    public OfferSettingService(IHttpContextAccessor httpContextAccessor, IHttpService httpService, IApiClientRepository apiClientRepository, IPartnerRepository partnerRepository) : base(httpContextAccessor)
    {
        _httpService = httpService;
        _apiClientRepository = apiClientRepository;
        _partnerRepository = partnerRepository;
    }

    #region Offer Setting
    public async Task<GenericResponse<bool>> AddOrUpdateOffer(OfferSettingRequestDto model)
    {
        try
        {
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
                StoreIds = model.StoreIds,
            };
            var result = await _partnerRepository.AddUpdateOfferSetting(offerSettingModel);
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
                    Message = StaticValues.Success
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
                });

            }
            return new GenericResponse<List<OfferResponseDto>>
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
    public async Task<GenericResponse<OfferDetailResponseDto>> GetOfferDetails(OfferDetailRequestDto model)
    {
        try
        {
            var offerSetting = await _partnerRepository.GetOfferSetting(ClientApiId, model.Id);
            if (offerSetting == null)
            {
                return new GenericResponse<OfferDetailResponseDto>
                {
                    Data = new OfferDetailResponseDto
                    {
                        Products = new List<BaseProductSearchResultDto>(),
                        Stores = new List<StoreDto>()
                    },
                    Message = StaticValues.Success
                };
            }
            OfferDetailResponseDto response = new OfferDetailResponseDto();
            if (model.IsProductLevel)
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
            if (model.IsStoreLevel)
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

}
