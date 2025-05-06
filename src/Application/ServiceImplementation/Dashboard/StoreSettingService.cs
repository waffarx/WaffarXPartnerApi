using Microsoft.AspNetCore.Http;
using MongoDB.Bson;
using MongoDB.Driver;
using WaffarXPartnerApi.Application.Common.DTOs.Dashboard;
using WaffarXPartnerApi.Application.Common.DTOs.Valu.SharedModels;
using WaffarXPartnerApi.Application.Common.DTOs.Valu.ValuRequestDto.GetStoresRequest;
using WaffarXPartnerApi.Application.Common.Models.SharedModels;
using WaffarXPartnerApi.Application.ServiceImplementation.Shared;
using WaffarXPartnerApi.Application.ServiceInterface;
using WaffarXPartnerApi.Application.ServiceInterface.Dashboard;
using WaffarXPartnerApi.Domain.Entities.NoSqlEnitities;
using WaffarXPartnerApi.Domain.Entities.NoSqlEntities;
using WaffarXPartnerApi.Domain.Models.SharedModels;
using WaffarXPartnerApi.Domain.RepositoryInterface.EntityFrameworkRepositoryInterface;

namespace WaffarXPartnerApi.Application.ServiceImplementation.Dashboard;
public class StoreSettingService : JWTUserBaseService, IStoreSettingService
{
    private readonly IMongoCollection<StoreSearchSetting> _storeSettingsCollection;
    private readonly IMongoCollection<StoreSearchSettingAudit> _storeSettingAuditCollection;
    private readonly IMongoCollection<StoreLookUp> _storeLookUpCollection;

    private readonly IApiClientRepository _apiClientRepository;
    private readonly IHttpService _httpService;


    public StoreSettingService(IMongoDatabase database, IHttpContextAccessor httpContextAccessor, IApiClientRepository apiClientRepository, IHttpService httpService) : base(httpContextAccessor)
    {
        _storeSettingsCollection = database.GetCollection<StoreSearchSetting>("StoreSearchSetting");
        _storeSettingAuditCollection = database.GetCollection<StoreSearchSettingAudit>("StoreSearchSettingAudit");
        _storeLookUpCollection = database.GetCollection<StoreLookUp>("StoreLookUp");
        _apiClientRepository = apiClientRepository;
        _httpService = httpService;
    }
    public async Task<GenericResponse<bool>> UpdateStoreSettingList(List<StoreSettingRequestDto> stores)
    {
        try
        {
            // Find the existing store setting by ClientId
            var filter = Builders<StoreSearchSetting>.Filter.Eq(s => s.ClientApiId, ClientApiId);
            var existingStoreSetting = await _storeSettingsCollection.Find(filter).FirstOrDefaultAsync();
            List<StoreSettings> storeSettings = new List<StoreSettings>();
            foreach (var store in stores)
            {
                storeSettings.Add(new StoreSettings
                {
                    IsFeatured = store.IsFeatured,
                    Rank = store.Rank,
                    StoreId = store.StoreId
                });
            }

            if (existingStoreSetting != null)
            {
                // Add an audit log for the store setting update
                var auditLog = new StoreSearchSettingAudit
                {
                    ClientApiId = ClientApiId,
                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = UserIdInt,
                    OriginalDocument = existingStoreSetting,
                };

                await _storeSettingAuditCollection.InsertOneAsync(auditLog);

                // Update the existing store setting
                var update = Builders<StoreSearchSetting>.Update
                    .Set(s => s.Stores, storeSettings)
                    .Set(s => s.UpdatedAt, DateTime.UtcNow)
                    .Set(s => s.UpdatedBy, UserIdInt);

                await _storeSettingsCollection.UpdateOneAsync(filter, update);
            }
            else
            {
                // Insert a new store setting if it doesn't exist
                var newStoreSetting = new StoreSearchSetting
                {
                    ClientApiId = ClientApiId,
                    SettingType = "whitelisedStores",
                    Stores = storeSettings,
                    CreatedBy = UserIdInt,
                    CreatedAt = DateTime.UtcNow,

                };

                await _storeSettingsCollection.InsertOneAsync(newStoreSetting);
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
    public async Task<GenericResponse<List<StoreResponseDto>>> GetWhiteListedStores()
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
                    });
                }
                return new GenericResponse<List<StoreResponseDto>>
                {
                    Status = StaticValues.Success,
                    Data = stores,
                };
            }
            return new GenericResponse<List<StoreResponseDto>>
            {
                Data = new List<StoreResponseDto>(),
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
            var pipeline = new[]
            {
                new BsonDocument("$match", new BsonDocument
                {
                    { "IsActive", true },
                }),
                new BsonDocument("$project", new BsonDocument
                {
                    { "_id", 0 },
                    { "StoreId", 1 },
                    { "StoreGuid", 1 },
                    { "NameAr", 1 },
                    { "NameEn", 1 },
                    { "LogoUrl", 1 },
                    { "LogoPngUrl", 1 }
                })
            };
            // Execute the aggregation pipeline
            var result = await _storeLookUpCollection.Aggregate<StoreLookUpResponseDto>(pipeline).ToListAsync();
            // Return the response
            return new GenericResponse<List<StoreLookUpResponseDto>>
            {
                Data = result,
                Message = StaticValues.Success,
            };

        }
        catch (Exception)
        {
            throw;
        }
    }
}
