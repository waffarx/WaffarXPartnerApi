namespace WaffarXPartnerApi.Domain.Models.PartnerMongoModels.TeamModel;
public class UpdateTeamWithActionModel : BaseTeamActionModel
{
    public Guid UpdatedBy { get; set; }
    public string UpdatedByUserName { get; set; }
    public Guid TeamId { get; set; }
}
