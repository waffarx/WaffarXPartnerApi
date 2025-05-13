namespace WaffarXPartnerApi.Application.Common.DTOs.Dashboard.Team.UpdateTeam;
public class UpdateTeamDto
{
    public string TeamId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public List<string> PageActionIds { get; set; }
}
