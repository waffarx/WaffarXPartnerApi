namespace WaffarXPartnerApi.Domain.Models.PartnerSqlModels;
public class UserPageActionsModel
{
    public Guid PageId { get; set; }
    public string PageName { get; set; }
    public string PageDescription { get; set; }
    public List<PageActionModel> Actions { get; set; }
}
