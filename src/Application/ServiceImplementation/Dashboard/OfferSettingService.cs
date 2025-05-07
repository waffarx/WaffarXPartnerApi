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
using WaffarXPartnerApi.Domain.Entities.NoSqlEnitities;
using WaffarXPartnerApi.Domain.Models.SharedModels;
using WaffarXPartnerApi.Domain.RepositoryInterface.EntityFrameworkRepositoryInterface;
using WaffarXPartnerApi.Domain.RepositoryInterface.MongoRepositoryInterface;

namespace WaffarXPartnerApi.Application.ServiceImplementation.Dashboard;
public class OfferSettingService : JWTUserBaseService, IOfferSettingService
{
    private readonly IMongoCollection<OfferSetting> _offerSettingsCollection;
    private readonly IMongoCollection<OfferLookUp> _offerLookupsCollection;
    private readonly IMongoCollection<StoreLookUp> _storeLookUpCollection;

    private readonly IHttpService _httpService;

    private readonly IApiClientRepository _apiClientRepository;
    private readonly IPartnerRepository _partnerRepository;


    public OfferSettingService(IMongoDatabase database, IHttpContextAccessor httpContextAccessor, IHttpService httpService, IApiClientRepository apiClientRepository, IPartnerRepository partnerRepository) : base(httpContextAccessor)
    {
        _offerSettingsCollection = database.GetCollection<OfferSetting>("OfferSetting");
        _offerLookupsCollection = database.GetCollection<OfferLookUp>("OfferLookUp");
        _storeLookUpCollection = database.GetCollection<StoreLookUp>("StoreLookUp");
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
            var result =   await _partnerRepository.AddUpdateOfferSetting(offerSettingModel);
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
            // Filter OfferSettings by ClientApiId  
            var filter = Builders<OfferSetting>.Filter.Eq(s => s.ClientApiId, ClientApiId);
            var offerSettings = await _offerSettingsCollection.Find(filter).ToListAsync();

            if (offerSettings == null || !offerSettings.Any())
            {
                return new GenericResponse<List<OfferResponseDto>>
                {
                    Data = new List<OfferResponseDto>(),
                };
            }

            var response = new List<OfferResponseDto>();

            foreach (var offerSetting in offerSettings)
            {
                // Fetch the associated OfferLookUp  
                var offerLookup = await _offerLookupsCollection.Find(l => l.Id == offerSetting.OfferLookUpId).FirstOrDefaultAsync();

                // Map the data to the response DTO  
                response.Add(new OfferResponseDto
                {
                    OfferId = offerSetting.Id.ToString(), // Convert ObjectId to Guid  
                    OfferName = IsEnglish ? offerLookup?.NameEn : offerLookup?.NameAr,
                    StartDate = offerSetting.StartDate,
                    EndDate = offerSetting.EndDate,
                    IsProductLevel = offerSetting.IsProductLevel,
                    IsStoreLevel = offerSetting.IsStoreLevel,
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
            var offerSetting =  await _partnerRepository.GetOfferSetting(ClientApiId, model.Id);
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
                var storeFilter = Builders<StoreLookUp>.Filter.In(s => s.StoreId, offerSetting.StoreIds);
                var storeLookUps = await _storeLookUpCollection.Find(storeFilter).ToListAsync();
                response.Stores = new List<StoreDto>();
                if (storeLookUps != null && storeLookUps.Any())
                {
                    foreach (var store in storeLookUps)
                    {
                        response.Stores.Add(new StoreDto
                        {
                            Id = store.StoreGuid,
                            Name = IsEnglish ? store.NameEn : store.NameAr,
                            Logo = store.LogoUrl,
                            LogoPng = store.LogoPngUrl
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
            if (model.Id == null)
            {
                await _offerLookupsCollection.InsertOneAsync(new OfferLookUp
                {
                    ClientApiId = ClientApiId,
                    NameAr = model.NameAr,
                    NameEn = model.NameEn,
                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = UserIdInt,
                    UniqueKey = model.NameEn.ToUpper() + model.NameAr.ToUpper(),
                });
            }
            else
            {
                var filter = Builders<OfferLookUp>.Filter.Eq(s => s.ClientApiId, ClientApiId) &
                             Builders<OfferLookUp>.Filter.Eq(s => s.Id, new ObjectId(model.Id));

                var update = Builders<OfferLookUp>.Update
                    .Set(s => s.NameAr, model.NameAr)
                    .Set(s => s.NameEn, model.NameEn)
                    .Set(s => s.UpdatedBy, UserIdInt);
                await _offerLookupsCollection.UpdateOneAsync(filter, update);
            }
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
            var filter = Builders<OfferLookUp>.Filter.Eq(s => s.ClientApiId, ClientApiId);
            var offerLookUp = await _offerLookupsCollection.Find(filter).ToListAsync();
            if (offerLookUp == null || !offerLookUp.Any())
            {
                return new GenericResponse<List<OfferLookUpResponsetDto>>
                {
                    Data = new List<OfferLookUpResponsetDto>(),
                };
            }
            var response = new List<OfferLookUpResponsetDto>();
            foreach (var offer in offerLookUp)
            {
                response.Add(new OfferLookUpResponsetDto
                {
                    Id = offer.Id.ToString(),
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
