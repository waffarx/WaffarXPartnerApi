namespace WaffarXPartnerApi.Application.Common.DTOs.Dashboard.User;
public class UserSearchResultDto
{
    public string UserId { get; set; }
    public string UserName { get; set; }
    public string Email { get; set; }
    public bool IsActive { get; set; }
    public List<string> Teams { get; set; }
}
