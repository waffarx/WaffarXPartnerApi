using WaffarXPartnerApi.Application.ServiceInterface.Shared;
using WaffarXPartnerApi.Domain.Models.SharedModels;
using WaffarXPartnerApi.Domain.RepositoryInterface.EntityFrameworkRepositoryInterface.WaffarX;

namespace WaffarXPartnerApi.Application.ServiceImplementation.Shared;
public class ResourceService : IResourceService
{
    private readonly IResourceRepository _resourceRepository;
    private readonly ICacheService _cacheService;
    public ResourceService(IResourceRepository resourceRepository, ICacheService cacheService)
    {
        _resourceRepository = resourceRepository;
        _cacheService = cacheService;
    }
    public async Task<string> GetResourceByKey(string resourceKey = "", bool IsEnglish = true)
    {
        var resValue = "";
        var ResModel = new List<ResourcesModel>();
        try
        {
            var cacheKey = "AvailableResourcesPartnerApi";
            ResModel = await _cacheService.GetOrSetCacheValueAsync
                (cacheKey, () => _resourceRepository.GetAllResources(), TimeSpan.FromHours(60));

            if (ResModel != null && ResModel.Count > 0)
            {
                var ResRetVal = ResModel.Where(a => a.Key == resourceKey).FirstOrDefault();
                if (ResRetVal != null)
                {
                    ResRetVal.AllValueAr = ResRetVal.ValueAr;
                    
                    if (ResRetVal != null)
                        resValue = IsEnglish ? ResRetVal.ValueEn : ResRetVal.AllValueAr;
                }
            }
            return resValue;

        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<Dictionary<string, string>> GetResourcesList(List<string> resKey, bool IsEnglish = true)
    {
        var ResValue = new Dictionary<string, string>();
        var ResModel = new List<ResourcesModel>();
        resKey = resKey.Select(x => x.ToLower()).ToList();
        try
        {
            var cacheKey = "AvailableResourcesPartnerApi";
            ResModel = await _cacheService.GetOrSetCacheValueAsync
               (cacheKey, () => _resourceRepository.GetAllResources(), TimeSpan.FromHours(60));


            if (ResModel != null && ResModel.Count > 0)
            {
                var ResourceListValues = ResModel.Where(a => resKey.Contains(a.Key.ToLower())).Distinct().ToList();

                if (ResourceListValues != null && ResourceListValues.Count > 0)
                {
                    ResourceListValues.ForEach(a => { a.AllValueAr = a.ValueAr; });

                    ResValue = ResourceListValues.ToDictionary(a => a.Key, a => IsEnglish ? a.ValueEn : a.AllValueAr, StringComparer.OrdinalIgnoreCase);
                }
            }
        }
        catch (Exception)
        {
        }
        return ResValue;
    }
}
