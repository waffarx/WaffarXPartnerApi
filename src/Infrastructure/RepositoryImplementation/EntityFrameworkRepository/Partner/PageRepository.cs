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
            var pages = await _waffarXPartnerDbContext.ClientPages
                                .Include(x => x.Page)
                                    .ThenInclude(x => x.PageActions)
                             .Where(x => x.ClientApiId == clientApiId && x.IsActive && !x.Page.IsSuperAdminPage)
                             .Select(x => new PageDetailModel
                             {
                                 Page =

                                     new PageModel
                                     {
                                         Id = x.Page.Id,
                                         Name = x.Page.PageName,
                                         Description = x.Page.Description,
                                         PageActions = x.Page.PageActions.Select(a => new PageActionModel
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

    public async Task<PageModel> GetPageAsync(string pageName, int clientApiId)
    {
        try
        {
            var page = await _waffarXPartnerDbContext.ClientPages
                .Include(x => x.Page)
                    .ThenInclude(x => x.PageActions)
                .FirstOrDefaultAsync(x => x.Page.PageName == pageName 
                    && x.ClientApiId == clientApiId
                    && x.IsActive);
            if (page == null)
            {
                return null;
            }
            return new PageModel
            {
                Id = page.Page.Id,
                Name = page.Page.PageName,
                Description = page.Page.Description,
                IsActive = page.IsActive,
                IsSuperAdminPage = page.Page.IsSuperAdminPage,
                PageActions = page.Page.PageActions.Select(a => new PageActionModel
                {
                    Id = a.Id,
                    Name = a.ActionName,
                    Description = a.Description,
                }).ToList()
            };

        }
        catch (Exception)
        {
            throw;
        }
    }
}
