using System.Security.Claims;
using WaffarXPartnerApi.Domain.Entities.SqlEntities.PartnerEntities;

namespace WaffarXPartnerApi.Application.ServiceInterface;


public interface IJwtService
{
    Task<string> GenerateAccessTokenAsync(User user);
    Task<ClaimsPrincipal> ValidateTokenAsync(string token);
}
