namespace WaffarXPartnerApi.Web.Infrastructure;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
public class RequiresPermissionAttribute : Attribute
{
    public string PageName { get; }
    public string ActionName { get; }

    public RequiresPermissionAttribute(string pageName, string actionName)
    {
        PageName = pageName ?? throw new ArgumentNullException(nameof(pageName));
        ActionName = actionName ?? throw new ArgumentNullException(nameof(actionName));
    }
}
