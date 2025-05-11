using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WaffarXPartnerApi.Application.ServiceInterface.Dashboard;

namespace WaffarXPartnerApi.Web.Endpoints;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class SharedController : ControllerBase
{
    private readonly ICommonService _commenService;
    public SharedController(ICommonService commenService)
    {
        _commenService = commenService;
    }
    [HttpGet("pages")]
    public async Task<IActionResult> GetPagesWithAction()
    {
        try
        {
            var pages = await _commenService.GetPages();
            return Ok(pages);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}
