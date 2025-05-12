using WaffarXPartnerApi.Domain.Models.PartnerSqlModels;

namespace WaffarXPartnerApi.Domain.Models.PartnerMongoModels.TeamModel;
public class TeamDetailsModel : TeamModel
{
    public List<UserModel> Users { get; set; } = new List<UserModel>();
    public List<PageDetailModel> Pages { get; set; }
}
