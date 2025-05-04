namespace WaffarXPartnerApi.Domain.RepositoryInterface.EntityFrameworkRepositoryInterface.WaffarX;
public interface IResourceRepository
{
    Task<List<ResourcesModel>> GetAllResources();
}
