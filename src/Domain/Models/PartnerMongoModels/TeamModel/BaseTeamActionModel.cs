namespace WaffarXPartnerApi.Domain.Models.PartnerMongoModels.TeamModel;
public class BaseTeamActionModel
{
    public string Name { get; set; }
    public string Description { get; set; }
    public int ClientApiId { get; set; }
    public List<Guid> PageActionIds { get; set; } = new List<Guid>();
}
