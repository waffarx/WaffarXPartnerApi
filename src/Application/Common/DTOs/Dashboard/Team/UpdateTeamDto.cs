namespace WaffarXPartnerApi.Application.Common.DTOs.Dashboard.Team;
public class UpdateTeamDto
{
    public string TeamId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public List<string> PageActionIds { get; set; }
    //public Guid UpdatedBy { get; set; }
    //public int ClientApiId { get; set; }
}
