using WaffarXPartnerApi.Application.ServiceInterface.Shared;
using WaffarXPartnerApi.Domain.RepositoryInterface.EntityFrameworkRepositoryInterface.WaffarX;
namespace WaffarXPartnerApi.Application.ServiceImplementation.Shared;
public class CacheService : ICacheService
{
    private readonly ICacheRepository _cacheRepository;
    public CacheService(ICacheRepository cacheRepository)
    {
        _cacheRepository = cacheRepository;
    }

    public async Task<T> GetOrSetCacheValueAsync<T>(string key, Func<Task<T>> fetchFunction, TimeSpan expiry)
    {
        var cachedValue = await _cacheRepository.GetAsync<T>(key);
        if (cachedValue == null || EqualityComparer<T>.Default.Equals(cachedValue, default))
        {
            cachedValue = await fetchFunction();
            await _cacheRepository.SetAsync(key, cachedValue, expiry);
        }
        return cachedValue;
    }
}
