namespace WaffarXPartnerApi.Domain.RepositoryInterface.EntityFrameworkRepositoryInterface.WaffarX;
public interface ICacheRepository
{
    Task<T> GetAsync<T>(string key);
    Task<List<T>> GetListAsync<T>(string key);
    Task<Dictionary<string, object>> GetDictionaryAsync(string[] keysList);
    Task SetAsync<T>(string key, T value, TimeSpan? expiration = null);
    Task RemoveAsync(string key);
    Task FlushCache();
    Task<string> GetDevKeys();
}
