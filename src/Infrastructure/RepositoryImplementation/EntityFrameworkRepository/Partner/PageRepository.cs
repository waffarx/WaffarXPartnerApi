using Microsoft.EntityFrameworkCore;
using WaffarXPartnerApi.Domain.Models.PartnerSqlModels;
using WaffarXPartnerApi.Domain.RepositoryInterface.EntityFrameworkRepositoryInterface.Partner;
using WaffarXPartnerApi.Infrastructure.Data;

namespace WaffarXPartnerApi.Infrastructure.RepositoryImplementation.EntityFrameworkRepository.Partner;
public class PageRepository : IPageRepository
{
    private readonly WaffarXPartnerDbContext _waffarXPartnerDbContext;
    public PageRepository(WaffarXPartnerDbContext waffarXPartnerDbContext)
    {
        _waffarXPartnerDbContext = waffarXPartnerDbContext;
    }
    public async Task<List<PageDetailModel>> GetPagesWithAction(int clientApiId)
    {
        try
        {
            var pages = await _waffarXPartnerDbContext.Pages
                             .Where(x => x.ClientApiId == clientApiId && x.IsActive && !x.IsSuperAdminPage)
                             .Select(x => new PageDetailModel
                             {
                                 Page =

                                     new PageModel
                                     {
                                         Id = x.Id,
                                         Name = x.PageName,
                                         Description = x.Description,
                                         PageActions = x.PageActions.Select(a => new PageActionModel
                                         {
                                             Id = a.Id,
                                             Name = a.ActionName,
                                             Description = a.Description
                                         }).ToList()
                                     }

                             }).ToListAsync();
            return pages;
        }
        catch (Exception)
        {
            throw;
        }

    }
}
