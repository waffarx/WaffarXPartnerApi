using WaffarXPartnerApi.Application.Common.DTOs.Dashboard.Team;

namespace WaffarXPartnerApi.Application.Common.DTOs.Dashboard.User;
public class UserDetailDto
{
    public Guid Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string UserName { get; set; }
    public List<TeamDto> Teams { get; set; }
}
