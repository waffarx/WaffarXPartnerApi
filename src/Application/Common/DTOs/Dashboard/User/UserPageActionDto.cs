namespace WaffarXPartnerApi.Application.Common.DTOs.Dashboard.User;
public class UserPageActionDto
{
    public string PageId { get; set; }
    public string PageName { get; set; }
    public string PageDescription { get; set; }
    public List<PageActionDto> Actions { get; set; }
}
public class PageActionDto
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
}
