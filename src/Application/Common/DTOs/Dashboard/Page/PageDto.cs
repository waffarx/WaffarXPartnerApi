using WaffarXPartnerApi.Application.Common.DTOs.Dashboard.User;

namespace WaffarXPartnerApi.Application.Common.DTOs.Dashboard.Page;
public class PageDto
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public List<PageActionDto> PageActions { get; set; }
}
