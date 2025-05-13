namespace WaffarXPartnerApi.Application.Common.DTOs.Dashboard.Team.CreateTeam;
public class CreateTeamDto
{
    public string Name { get; set; }
    public string Description { get; set; }
    public List<string> PageActionIds { get; set; }

}
