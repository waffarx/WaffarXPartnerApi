using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WaffarXPartnerApi.Application.ServiceInterface.Dashboard;
using WaffarXPartnerApi.Domain.Enums;

namespace WaffarXPartnerApi.Web.Endpoints;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class ReportsController : ControllerBase
{
    private readonly IReportService _reportService;
    public ReportsController(IReportService reportService)
    {
        _reportService = reportService;
    }
    
    [HttpGet("partnerordersdata")]
    [RequiresPermission(nameof(AdminPageEnum.Reports), nameof(AdminActionEnum.ListReportCards))]

    public async Task<IActionResult> GetPartnerOrdersData()
    {
        var response = await _reportService.GetPartnerOrdersStatistics();
        return Ok(response);

    }
}
