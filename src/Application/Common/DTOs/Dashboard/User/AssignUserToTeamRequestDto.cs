namespace WaffarXPartnerApi.Application.Common.DTOs.Dashboard.User;
public class AssignUserToTeamRequestDto
{
    public string UserId { get; set; }
    public List<string> TeamIds { get; set; }
}
