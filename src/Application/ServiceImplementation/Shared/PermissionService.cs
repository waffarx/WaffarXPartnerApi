using Microsoft.AspNetCore.Http;
using WaffarXPartnerApi.Application.ServiceInterface.Shared;
using WaffarXPartnerApi.Domain.RepositoryInterface.EntityFrameworkRepositoryInterface.Partner;

namespace WaffarXPartnerApi.Application.ServiceImplementation.Shared;
public class PermissionService : JWTUserBaseService, IPermissionService
{
    private readonly IUserRepository _userRepository;
    private readonly IPageRepository _pageRepository;

    public PermissionService(IHttpContextAccessor httpContextAccessor, IUserRepository userRepository, IPageRepository pageRepository) : base(httpContextAccessor)
    {
        _userRepository = userRepository;
        _pageRepository = pageRepository;
    }

    public async Task<bool> UserHasPermissionAsync(string pageName, string actionName)
    {
        // First, get user info to check if they're a super admin
        var user = await _userRepository.GetByIdAsync(UserId);
        if (user == null)
            return false;

        // Super admins have access to everything
        if (user.IsSuperAdmin)
            return true;

        // Find the page and action by name
        var page = await _pageRepository.GetPageAsync(pageName,ClientApiId);
        if (page == null)
            return false;

        // Check if this is a super admin page that regular users can't access
        if (page.IsSuperAdminPage && !user.IsSuperAdmin)
            return false;

        if (page.PageActions == null)
            return false;

        // Get all teams the user belongs to
        var userTeams = await _userRepository.GetUserPageAndPageAction(UserId);

        var pageActionGuids = page.PageActions.Select(x => x.Id).ToList();
        // Check if any of the user's teams have access to this page action
        var hasAccess = userTeams.Any(x => x.PageId == page.Id && x.Actions.Any(action => pageActionGuids.Contains(action.Id)));

        return hasAccess;
    }
}
