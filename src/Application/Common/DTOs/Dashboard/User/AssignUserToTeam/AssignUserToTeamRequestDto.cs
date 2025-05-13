namespace WaffarXPartnerApi.Application.Common.DTOs.Dashboard.User.AssignUserToTeam;
public class AssignUserToTeamRequestDto
{
    public string UserId { get; set; }
    public List<string> TeamIds { get; set; }
}
