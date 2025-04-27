using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using WaffarXPartnerApi.Application.ServiceInterface;

namespace WaffarXPartnerApi.Application.ServiceImplementation;
public class HttpService : IHttpService
{
    private readonly HttpClient _httpClient;
    private readonly JsonSerializerOptions _jsonOptions;

    public HttpService(HttpClient httpClient)
    {
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));

        _jsonOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };
    }

    public async Task<T> GetAsync<T>(string url, Dictionary<string, string> headers = null)
    {
        return await SendAsync<T>(HttpMethod.Get, url, null, headers);
    }

    public async Task<T> PostAsync<T>(string url, object data, Dictionary<string, string> headers = null)
    {
        return await SendAsync<T>(HttpMethod.Post, url, data, headers);
    }

    public async Task<T> PutAsync<T>(string url, object data, Dictionary<string, string> headers = null)
    {
        return await SendAsync<T>(HttpMethod.Put, url, data, headers);
    }

    public async Task<T> DeleteAsync<T>(string url, Dictionary<string, string> headers = null)
    {
        return await SendAsync<T>(HttpMethod.Delete, url, null, headers);
    }

    public async Task<T> SendAsync<T>(HttpMethod method, string url, object data = null, Dictionary<string, string> headers = null)
    {
        var request = new HttpRequestMessage(method, url);

        // Add content for POST, PUT, etc.
        if (data != null && (method == HttpMethod.Post || method == HttpMethod.Put || method == HttpMethod.Patch))
        {
            var json = JsonSerializer.Serialize(data, _jsonOptions);
            request.Content = new StringContent(json, Encoding.UTF8, "application/json");
        }

        // Add headers
        if (headers != null)
        {
            foreach (var header in headers)
            {
                if (header.Key.ToLower() == "content-type" && request.Content != null)
                {
                    // Only set ContentType if Content exists
                    request.Content.Headers.ContentType = new MediaTypeHeaderValue(header.Value);
                }
                else
                {
                    request.Headers.Add(header.Key, header.Value);
                }
            }
        }

        var response = await _httpClient.SendAsync(request);

        // Ensure we got a successful response
        response.EnsureSuccessStatusCode();

        // Handle empty response
        if (response.Content.Headers.ContentLength == 0)
        {
            return default;
        }

        // Parse the response
        var content = await response.Content.ReadAsStringAsync();

        if (typeof(T) == typeof(string))
        {
            return (T)(object)content;
        }

        return JsonSerializer.Deserialize<T>(content, _jsonOptions);
    }
}
