using WaffarXPartnerApi.Domain.Models.PartnerSqlModels;

namespace WaffarXPartnerApi.Domain.RepositoryInterface.EntityFrameworkRepositoryInterface.Partner;
public interface IPageRepository
{
    Task<List<PageDetailModel>> GetPagesWithAction(int clientApiId);
    Task<PageModel> GetPageAsync(string pageName, int clientApiId);

}
