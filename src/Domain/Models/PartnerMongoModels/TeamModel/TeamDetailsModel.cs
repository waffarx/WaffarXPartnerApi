namespace WaffarXPartnerApi.Domain.Models.PartnerMongoModels.TeamModel;
public class TeamDetailsModel : TeamModel
{
    public List<UserModel> Users { get; set; } = new List<UserModel>();
}
