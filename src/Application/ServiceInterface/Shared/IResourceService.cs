namespace WaffarXPartnerApi.Application.ServiceInterface.Shared;
public interface IResourceService
{
    Task<string> GetResourceByKey(string resourceKey = "", bool IsEnglish = true);
    Task<Dictionary<string, string>> GetResourcesList(List<string> resKey, bool IsEnglish = true);
}
