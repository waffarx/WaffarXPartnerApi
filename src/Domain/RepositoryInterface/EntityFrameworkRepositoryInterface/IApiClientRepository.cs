using WaffarXPartnerApi.Domain.Entities.SqlEntities.WaffarXEntities;

namespace WaffarXPartnerApi.Domain.RepositoryInterface.EntityFrameworkRepositoryInterface;
public interface IApiClientRepository
{
    Task<ApiClient> GetClientById(string Id);
    Task<long> GetUserIdByClient(string Id);

}
