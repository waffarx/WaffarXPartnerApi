using WaffarXPartnerApi.Application.Common.DTOs;
using WaffarXPartnerApi.Application.Common.Models.SharedModels;
using WaffarXPartnerApi.Domain.Entities.SqlEntities.PartnerEntities;

namespace WaffarXPartnerApi.Application.ServiceInterface.Dashboard;
public interface IAuthService
{
    Task<GenericResponse<TokenResponseDto>> LoginAsync(string username, string password);
    Task<GenericResponse<TokenResponseDto>> RegisterAsync(RegisterRequestDto user);
    Task<TokenResponse> RefreshTokenAsync(string refreshToken);
    Task<bool> RevokeTokenAsync(string refreshToken);
    Task<TokenValidationResult> ValidateJwtTokenAsync(string token);
    Task<GenericResponse<bool>> DeactivateUser(string userId);
}
public class TokenResponse
{
    public bool Success { get; set; }
    public string AccessToken { get; set; }
    public string RefreshToken { get; set; }
    public DateTime AccessTokenExpiresAt { get; set; }
    public DateTime RefreshTokenExpiresAt { get; set; }
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
