using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace WaffarXPartnerApi.Application.ServiceImplementation.Shared;
public class JWTUserBaseService
{
    public int ClientApiId { get; set; }
    public string UserLanguage { get; set; }
    public bool IsEnglish { get; set; }

    // User claims from JWT token  
    public Guid UserId { get; set; }
    public int UserIdInt { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public bool IsSuperAdmin { get; set; }
    public List<Guid> TeamIds { get; set; } = new List<Guid>();
    public bool IsAuthenticated { get; set; }

    private readonly IHttpContextAccessor _httpContextAccessor;
    public JWTUserBaseService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;

        if (_httpContextAccessor.HttpContext != null)
        {
            // Extract user claims from JWT token  
            var user = _httpContextAccessor.HttpContext.User;
            IsAuthenticated = user?.Identity?.IsAuthenticated ?? false;

            if (IsAuthenticated)
            {
                var userIdValue = user.FindFirstValue("userId");
                if (Guid.TryParse(userIdValue, out var parsedUserId))
                {
                    UserId = parsedUserId;
                }

                var userIdIntValue = user.FindFirstValue("userIdInt");
                if (int.TryParse(userIdIntValue, out var parsedUserIdInt))
                {
                    UserIdInt = parsedUserIdInt;
                }

                Username = user.FindFirstValue(ClaimTypes.NameIdentifier);
                Email = user.FindFirstValue(ClaimTypes.Email);
                FirstName = user.FindFirstValue("firstName");
                LastName = user.FindFirstValue("lastName");

                var clientApiIdClaim = user.FindFirstValue("clientApiId");
                if (!string.IsNullOrEmpty(clientApiIdClaim) && int.TryParse(clientApiIdClaim, out var tokenClientApiId))
                {
                    ClientApiId = tokenClientApiId;
                }

                IsSuperAdmin = user.IsInRole("SuperAdmin");

                // Extract team IDs  
                var teamClaims = user.FindAll("team");
                if (teamClaims != null && teamClaims.Any())
                {
                    foreach (var teamClaim in teamClaims)
                    {
                        if (!string.IsNullOrEmpty(teamClaim.Value) && Guid.TryParse(teamClaim.Value, out var teamId))
                        {
                            TeamIds.Add(teamId);
                        }
                    }
                }
              
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
        else
        {
            // Default values when HttpContext is null  
            UserLanguage = "en-US";
            IsEnglish = true;
        }
    }
}
