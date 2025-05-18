namespace WaffarXPartnerApi.Domain.Models.PartnerSqlModels;
public class PageModel
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public bool IsActive { get; set; }
    public bool IsSuperAdminPage { get; set; }
    public List<PageActionModel> PageActions { get; set; }
}
