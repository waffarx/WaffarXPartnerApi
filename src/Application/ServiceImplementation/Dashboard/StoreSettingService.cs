using Microsoft.AspNetCore.Http;
using MongoDB.Driver;
using WaffarXPartnerApi.Application.Common.DTOs.Dashboard;
using WaffarXPartnerApi.Application.Common.DTOs.Valu.ValuRequestDto.GetStoresRequest;
using WaffarXPartnerApi.Application.Common.Models.SharedModels;
using WaffarXPartnerApi.Application.ServiceImplementation.Shared;
using WaffarXPartnerApi.Application.ServiceInterface.Dashboard;
using WaffarXPartnerApi.Application.ServiceInterface.Shared;
using WaffarXPartnerApi.Domain.Models.PartnerMongoModels;
using WaffarXPartnerApi.Domain.Models.SharedModels;
using WaffarXPartnerApi.Domain.RepositoryInterface.EntityFrameworkRepositoryInterface;
using WaffarXPartnerApi.Domain.RepositoryInterface.EntityFrameworkRepositoryInterface.WaffarX;
using WaffarXPartnerApi.Domain.RepositoryInterface.MongoRepositoryInterface;

namespace WaffarXPartnerApi.Application.ServiceImplementation.Dashboard;
public class StoreSettingService : JWTUserBaseService, IStoreSettingService
{
    private readonly IApiClientRepository _apiClientRepository;
    private readonly IHttpService _httpService;
    private readonly ICacheRepository _cacheRepository;

    private readonly IPartnerRepository _partnerRepository;


    public StoreSettingService(IHttpContextAccessor httpContextAccessor, IApiClientRepository apiClientRepository, IHttpService httpService, ICacheRepository cacheRepository, IPartnerRepository partnerRepository) : base(httpContextAccessor)
    {
        _apiClientRepository = apiClientRepository;
        _httpService = httpService;
        _cacheRepository = cacheRepository;
        _partnerRepository = partnerRepository;
    }
    public async Task<GenericResponse<bool>> UpdateStoreSettingList(List<StoreSettingRequestDto> stores)
    {
        try
        {
            UpdateWhiteListStoreRequestModel model = new UpdateWhiteListStoreRequestModel
            {
                ClientApiId = ClientApiId,
                UserId = UserIdInt,
                StoreList = stores.Select(store => new WhitelistStoreToUpdate
                {
                    Rank = store.Rank,
                    IsFeatured = store.IsFeatured,
                    StoreId = store.StoreId,
                }).ToList()

            };
            await _partnerRepository.UpdateWhiteListStore(model);
            // purge the cache key after updating the store settings
            var clientGuid = await _apiClientRepository.GetClientGuidById(ClientApiId);
            string whiteListedStoresCacheKey = $"WhiteListedStores:{clientGuid}";
            string apiClientIdCacheKey = $"ApiClientId:{clientGuid}";
            await _cacheRepository.RemoveAsync(whiteListedStoresCacheKey);
            await _cacheRepository.RemoveAsync(apiClientIdCacheKey);
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
    public async Task<GenericResponse<List<WhiteListedStoreResonseDto>>> GetWhiteListedStores()
    {
        try
        {
            var headers = new Dictionary<string, string>
            {
                ["Content-Type"] = "application/json"
            };
            var clientGuid = await _apiClientRepository.GetClientGuidById(ClientApiId);
            GetStoresDto requestBody = new GetStoresDto
            {
                ClientApiId = clientGuid,
                IsEnglish = IsEnglish,
                ItemCount = 1000,
                PageNumber = 1,

            };
            // Make the POST request using our generic HTTP service
            var searchResults = await _httpService.PostAsync<GenericResponseWithCount<List<WhiteListedStoreResonseDto>>>(
                AppSettings.ExternalApis.ValuUrl + "GetStores",
                requestBody,
                headers);
            if (searchResults.Status == StaticValues.Success && searchResults.Data != null)
            {
                List<WhiteListedStoreResonseDto> stores = new List<WhiteListedStoreResonseDto>();
                foreach (var item in searchResults.Data)
                {
                    stores.Add(new WhiteListedStoreResonseDto
                    {
                        Id = item.Id,
                        Logo = item.Logo,
                        LogoPng = item.LogoPng,
                        Name = item.Name,
                        Rank = item.Rank,
                    });
                }
                return new GenericResponse<List<WhiteListedStoreResonseDto>>
                {
                    Status = StaticValues.Success,
                    Data = stores,
                };
            }
            return new GenericResponse<List<WhiteListedStoreResonseDto>>
            {
                Data = new List<WhiteListedStoreResonseDto>(),
                Status = StaticValues.Success,
            };
        }
        catch (Exception)
        {
            throw;
        }
    }
    public async Task<GenericResponse<List<StoreLookUpResponseDto>>> GetStoreLookUp()
    {
        try
        {
            var result = await _partnerRepository.GetAllStoreLookUp();
            if (result == null || !result.Any())
            {
                return new GenericResponse<List<StoreLookUpResponseDto>>
                {
                    Data = new List<StoreLookUpResponseDto>(),
                    Message = StaticValues.Success,
                };
            }
            // Map the result to the response DTO
            List<StoreLookUpResponseDto> storeLookUpResponse = result.Select(store => new StoreLookUpResponseDto
            {
                LogoPngUrl = store.LogoPng,
                LogoUrl = store.Logo,
                NameAr = store.NameAr,
                NameEn = store.NameEn,
                Id = store.Id,
            }).ToList();
            // Return the response
            return new GenericResponse<List<StoreLookUpResponseDto>>
            {
                Data = storeLookUpResponse,
                Message = StaticValues.Success,
            };

        }
        catch (Exception)
        {
            throw;
        }
    }
}
