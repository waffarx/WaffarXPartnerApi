namespace WaffarXPartnerApi.Application.Common.DTOs.Dashboard.User;
public class UserSearchRequestDto
{
    public string Email { get; set; }
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}
