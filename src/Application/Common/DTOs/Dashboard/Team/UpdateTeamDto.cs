namespace WaffarXPartnerApi.Application.Common.DTOs.Dashboard.Team;
public class UpdateTeamDto
{
    public Guid TeamId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public Guid UpdatedBy { get; set; }
    public List<Guid> PageActionIds { get; set; }
    public int ClientApiId { get; set; }
}
