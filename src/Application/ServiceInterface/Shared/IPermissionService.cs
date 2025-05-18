namespace WaffarXPartnerApi.Application.ServiceInterface.Shared;
public interface IPermissionService
{
    Task<bool> UserHasPermissionAsync(string pageName, string actionName);
}
