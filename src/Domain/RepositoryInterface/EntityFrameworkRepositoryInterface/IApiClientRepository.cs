﻿using WaffarXPartnerApi.Domain.Entities.SqlEntities.WaffarXEntities;

namespace WaffarXPartnerApi.Domain.RepositoryInterface.EntityFrameworkRepositoryInterface;
public interface IApiClientRepository
{
    Task<ApiClient> GetClientById(string Id);
    Task<long> GetUserIdByClient(string Id);
    Task<int> GetClientIdByGuid(string Id);
    Task<Guid> GetClientGuidById(int Id);
    Task<ApiClient> GetClientByName(string name);
    Task<bool> CreateClient(ApiClient entity);
    Task<long> GetUserIdByClientId(int ClientId);
}
