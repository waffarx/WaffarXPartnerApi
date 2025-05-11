using WaffarXPartnerApi.Application.Common.DTOs.Dashboard.User;

namespace WaffarXPartnerApi.Application.Common.DTOs;
public class TokenResponseDto
{
    public string AccessToken { get; set; }
    public string RefreshToken { get; set; }
    public DateTime AccessTokenExpiresAt { get; set; }
    public DateTime RefreshTokenExpiresAt { get; set; }
    public List<UserPageActionDto> UserPages { get; set; }
}
