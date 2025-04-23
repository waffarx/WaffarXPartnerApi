using WaffarXPartnerApi.Domain.Entities.SqlEntities.PartnerEntities;

namespace WaffarXPartnerApi.Application.ServiceInterface;
public interface IAuthService
{
    Task<TokenResponse> LoginAsync(string username, string password);
    Task<TokenResponse> RegisterAsync(User user, string password);
    Task<TokenResponse> RefreshTokenAsync(string refreshToken);
    Task<bool> RevokeTokenAsync(string refreshToken);
    Task<TokenValidationResult> ValidateJwtTokenAsync(string token);
}
public class TokenResponse
{
    public bool Success { get; set; }
    public string AccessToken { get; set; }
    public string RefreshToken { get; set; }
    public DateTime AccessTokenExpiresAt { get; set; }
    public DateTime RefreshTokenExpiresAt { get; set; }
    public UserDto User { get; set; }
    public string Message { get; set; }
}

public class TokenValidationResult
{
    public bool Success { get; set; }
    public User User { get; set; }
    public string Message { get; set; }
}

public class UserDto
{
    public Guid Id { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public bool IsSuperAdmin { get; set; }
    public int ClientApiId { get; set; }
}
