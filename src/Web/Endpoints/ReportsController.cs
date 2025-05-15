using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WaffarXPartnerApi.Application.ServiceInterface.Dashboard;

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
    public async Task<IActionResult> GetPartnerOrdersData()
    {
        var response = await _reportService.GetPartnerOrdersStatistics();
        return Ok(response);

    }
}
