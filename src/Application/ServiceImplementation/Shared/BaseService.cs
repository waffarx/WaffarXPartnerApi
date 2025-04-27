using Microsoft.AspNetCore.Http;

namespace WaffarXPartnerApi.Application.ServiceImplementation.Shared;
public class BaseService
{
    public Guid? ClientApiId { get; set; }
    public string UserLanguage { get; set; }
    public bool IsEnglish { get; set; }

    private readonly IHttpContextAccessor _httpContextAccessor;
    public BaseService()
    {

    }
    public BaseService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
        if (_httpContextAccessor.HttpContext != null)
        {
            if (_httpContextAccessor.HttpContext.Request.Headers.ContainsKey("wxc-id"))
            {
                if (Guid.TryParse(_httpContextAccessor.HttpContext.Request.Headers["wxc-id"].ToString(), out Guid parsedGuid))
                {
                    ClientApiId = parsedGuid;
                  
                }
                else
                {
                    ClientApiId = null;
                }
            }
            else
            {
                ClientApiId = null;
            }
            if (_httpContextAccessor.HttpContext.Request.Headers.ContainsKey("Accept-Language"))
            {
                UserLanguage = _httpContextAccessor.HttpContext.Request.Headers["Accept-Language"].ToString().Trim('"');
            }
            else
            {
                UserLanguage = "en-US";
            }

            IsEnglish = UserLanguage.ToLower().Contains("en");
        }
    }
}
