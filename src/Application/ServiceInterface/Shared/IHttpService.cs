namespace WaffarXPartnerApi.Application.ServiceInterface.Shared;
public interface IHttpService
{
    Task<T> GetAsync<T>(string url, Dictionary<string, string> headers = null);
    Task<T> PostAsync<T>(string url, object data, Dictionary<string, string> headers = null);
    Task<T> PutAsync<T>(string url, object data, Dictionary<string, string> headers = null);
    Task<T> DeleteAsync<T>(string url, Dictionary<string, string> headers = null);
    Task<T> SendAsync<T>(HttpMethod method, string url, object data = null, Dictionary<string, string> headers = null);
}
