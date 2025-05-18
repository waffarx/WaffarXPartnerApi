using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WaffarXPartnerApi.Application.ServiceInterface.Shared;

namespace WaffarXPartnerApi.Web.Infrastructure;

public class PermissionAuthorizationFilter : IAsyncAuthorizationFilter
{
    public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
    {
        // Skip authorization if action has AllowAnonymous attribute
        if (context.ActionDescriptor.EndpointMetadata.Any(em => em.GetType().Name == "AllowAnonymousAttribute"))
            return;

        // Get the permission requirements from attributes
        var controllerAttribute = context.ActionDescriptor.EndpointMetadata
            .OfType<RequiresPermissionAttribute>()
            .FirstOrDefault();

        if (controllerAttribute == null)
        {
            // No permission requirements - allow access
            return;
        }

        var permissionService = context.HttpContext.RequestServices.GetRequiredService<IPermissionService>();
        var hasPermission = await permissionService.UserHasPermissionAsync(
            controllerAttribute.PageName,
            controllerAttribute.ActionName);

        if (!hasPermission)
        {
            context.Result = new ForbidResult();
        }
    }
}
