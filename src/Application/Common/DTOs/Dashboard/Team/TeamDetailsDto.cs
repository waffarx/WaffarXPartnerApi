using WaffarXPartnerApi.Application.Common.DTOs.Dashboard.Page;

namespace WaffarXPartnerApi.Application.Common.DTOs.Dashboard.Team;
public class TeamDetailsDto : TeamDto
{
    public List<TeamUserDto> Users { get; set; }
    public List<PageDetailDto> Pages { get; set; }
}
