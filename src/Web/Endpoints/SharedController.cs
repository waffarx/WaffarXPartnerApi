using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WaffarXPartnerApi.Application.Common.DTOs.Dashboard.Postback;
using WaffarXPartnerApi.Application.ServiceInterface.Dashboard;
using WaffarXPartnerApi.Domain.Constants;
using WaffarXPartnerApi.Domain.Enums;

namespace WaffarXPartnerApi.Web.Endpoints;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class SharedController : ControllerBase
{
    private readonly ICommonService _commenService;
    private readonly IReportService _reportService;
    public SharedController(ICommonService commenService, IReportService reportService)
    {
        _commenService = commenService;
        _reportService = reportService; 
    }
    [HttpGet("pages")]
    public async Task<IActionResult> GetPagesWithAction()
    {
        var pages = await _commenService.GetPages();
        return Ok(pages);
    }

    [HttpGet("partnerurl")]
    [RequiresPermission(AdminPageConstants.PostbackUrl, AdminActionConstants.GetPostbackUrl)]

    public async Task<IActionResult> GetPartnerPostbacks()
    {
        var Url = await _reportService.GetPartnerPostbackUrl();
        return Ok(Url);
    }

    [HttpPost("savepostback")]
    [RequiresPermission(AdminPageConstants.PostbackUrl, AdminActionConstants.SavePostbackUrl)]
    public async Task<IActionResult> SavePostback(PostbackDto dto)
    {
        var Url = await _reportService.AddOrUpdatePostback(dto);
        return Ok(Url);
    }
}
