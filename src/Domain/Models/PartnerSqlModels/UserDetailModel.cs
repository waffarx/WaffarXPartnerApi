using WaffarXPartnerApi.Domain.Models.PartnerMongoModels.TeamModel;

namespace WaffarXPartnerApi.Domain.Models.PartnerSqlModels;
public class UserDetailModel
{
    public Guid Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string UserName { get; set; }
    public List<TeamModel> Teams { get; set; }
}
