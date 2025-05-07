namespace WaffarXPartnerApi.Domain.RepositoryInterface.EntityFrameworkRepositoryInterface.WaffarX;
public interface IAdvertiserRepository
{
    Task<List<int>> GetStoreIds(List<Guid> Guids);
    Task<List<int>> GetDisabledStores();
   
}
