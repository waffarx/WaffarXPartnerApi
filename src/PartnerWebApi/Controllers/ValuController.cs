using Microsoft.AspNetCore.Mvc;
using WaffarXPartnerApi.Application.Common.DTOs.ValuRequestDto;
using WaffarXPartnerApi.Application.ServiceInterface;

namespace PartnerWebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ValuController : ControllerBase
{
    private readonly IValuService _valuService;

    public ValuController(IValuService valuService)
    {
        _valuService = valuService;
    }

    [HttpPost("search")]
    public async Task<IActionResult> Search(ProductSearchDto query)
    {
        var result = await _valuService.SearchProduct(query);
        return Ok(result);
    }
}
