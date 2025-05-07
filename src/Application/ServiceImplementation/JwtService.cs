using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using WaffarXPartnerApi.Application.ServiceInterface;
using WaffarXPartnerApi.Domain.Entities.SqlEntities.PartnerEntities;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using WaffarXPartnerApi.Application.Common.Models.SharedModels;

namespace WaffarXPartnerApi.Application.ServiceImplementation;
public class JwtService : IJwtService
{
    private readonly JwtSettings _jwtSettings;

    public JwtService(IOptions<JwtSettings> jwtSettings)
    {
        _jwtSettings = jwtSettings.Value;
    }

    public Task<string> GenerateAccessTokenAsync(User user)
    {
        var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Username),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim("userId", user.Id.ToString()),
                new Claim("userIdInt", user.UserId.ToString()),
                new Claim("firstName", user.FirstName ?? string.Empty),
                new Claim("lastName", user.LastName ?? string.Empty),
                new Claim("clientApiId", user.ClientApiId.ToString()),

            };

        if (user.IsSuperAdmin)
        {
            claims.Add(new Claim(ClaimTypes.Role, "SuperAdmin"));
        }

        // Add claims for teams the user belongs to
        if (user.UserTeams != null)
        {
            foreach (var userTeam in user.UserTeams)
            {
                claims.Add(new Claim("team", userTeam.TeamId.ToString()));
            }
        }

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Secret));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var expires = DateTime.UtcNow.AddMinutes(_jwtSettings.AccessTokenExpirationMinutes);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = expires,
            SigningCredentials = creds,
            Issuer = _jwtSettings.Issuer,
            Audience = _jwtSettings.Audience
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);

        return Task.FromResult(tokenHandler.WriteToken(token));
    }

    public Task<ClaimsPrincipal> ValidateTokenAsync(string token)
    {
        try
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtSettings.Secret);

            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = true,
                ValidIssuer = _jwtSettings.Issuer,
                ValidateAudience = true,
                ValidAudience = _jwtSettings.Audience,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            };

            var principal = tokenHandler.ValidateToken(token, validationParameters, out var validatedToken);

            // Additional check to ensure token is a JWT
            if (!(validatedToken is JwtSecurityToken jwtToken) ||
                !jwtToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
            {
                return Task.FromResult<ClaimsPrincipal>(null);
            }

            return Task.FromResult(principal);
        }
        catch
        {
            return Task.FromResult<ClaimsPrincipal>(null);
        }
    }
}
public class JwtSettings
{
    public string Secret { get; set; } = AppSettings.JwtSettings.SecretKey;
    public string Issuer { get; set; } = AppSettings.JwtSettings.Issuer;
    public string Audience { get; set; } = AppSettings.JwtSettings.Audience;
    public int AccessTokenExpirationMinutes { get; set; } = AppSettings.JwtSettings.ExpiryInMinutes;
    public int RefreshTokenExpirationDays { get; set; } = AppSettings.JwtSettings.ExpiryInMinutes;
}
