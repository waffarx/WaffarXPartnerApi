using StackExchange.Redis;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text;
using WaffarXPartnerApi.Domain.RepositoryInterface.EntityFrameworkRepositoryInterface.WaffarX;

namespace WaffarXPartnerApi.Infrastructure.RepositoryImplementation.EntityFrameworkRepository.WaffarX;
public class RedisRepository : ICacheRepository
{
    private readonly IDatabase _database;


    public RedisRepository(IConnectionMultiplexer connectionMultiplexer)
    {
        _database = connectionMultiplexer.GetDatabase();
    }

    public async Task<T> GetAsync<T>(string key)
    {
        var value = await _database.StringGetAsync(key);
        if (value.IsNull)
            return default;
        else
            return JsonSerializer.Deserialize<T>(value);
    }

    public async Task<Dictionary<string, object>> GetDictionaryAsync(string[] keysList)
    {
        var result = new Dictionary<string, object>();

        foreach (var item in keysList)
        {
            var temp = await GetAsync<object>(item);
            result.Add(item, temp);
        }
        return result;
    }

    public async Task SetAsync<T>(string key, T value, TimeSpan? expiration = null)
    {
        var options = new JsonSerializerOptions
        {
            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
        };


        var serializedValue = JsonSerializer.Serialize(value, options);

        var utf8EncodedValue = Encoding.UTF8.GetBytes(serializedValue);

        await _database.StringSetAsync(key, utf8EncodedValue, expiration);
    }

    public async Task RemoveAsync(string key)
    {
        await _database.KeyDeleteAsync(key);
    }

    public async Task<List<T>> GetListAsync<T>(string key)
    {
        var serializedList = await _database.StringGetAsync(key);
        if (serializedList.IsNullOrEmpty) return new List<T>();

        // Deserialize the list back to List<T>
        var list = serializedList.ToString().Split(',');
        return list.Select(item => (T)Convert.ChangeType(item, typeof(T))).ToList();
    }

    public async Task FlushCache()
    {
        await _database.ExecuteAsync("FLUSHALL");
    }
    public async Task<string> GetDevKeys()
    {
        //var redis = ConnectionMultiplexer.Connect("waffarx-redis.redis.cache.windows.net:6379,password=Ostl53bI6EztHPSoBFiukl5hS2RAQQJ0EAzCaB786dM=");
        //var server = redis.GetServer("waffarx-redis.redis.cache.windows.net", 6379);
        var redis = ConnectionMultiplexer.Connect("localhost");
        var server = redis.GetServer("localhost", 6379);
        var res = new StringBuilder();
        // Iterate through keys using SCAN
        var cursor = 0;
        await Task.Delay(10);
        var result = server.Keys(cursor, "*Dev", 100);
        foreach (var key in result)
        {
            res.Append(key.ToString() + " |");
        }

        return res.ToString();
    }
}
