namespace WaffarXPartnerApi.Application.Common.DTOs;
public class TokenResponseDto
{
    public bool Success { get; set; }
    public string AccessToken { get; set; }
    public string RefreshToken { get; set; }
    public DateTime AccessTokenExpiresAt { get; set; }
    public DateTime RefreshTokenExpiresAt { get; set; }
    public UserDto User { get; set; }
    public string Message { get; set; }
}
