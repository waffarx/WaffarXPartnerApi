namespace WaffarXPartnerApi.Application.Common.DTOs.Dashboard.Team;
public class CreateTeamDto
{
    public string Name { get; set; }
    public string Description { get; set; }
    public List<string> PageActionIds { get; set; }
    // public Guid CreatedBy { get; set; }
    //   public int ClientApiId { get; set; }
}
