namespace WaffarXPartnerApi.Application.ServiceInterface.Shared;
public interface ICacheService
{
    Task<T> GetOrSetCacheValueAsync<T>(string key, Func<Task<T>> fetchFunction, TimeSpan expiry);
}
