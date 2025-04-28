using Microsoft.AspNetCore.Mvc;
using PartnerWebApi.Infrastructure;
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
    [CompressResponse]  // Apply compression only to this endpoint

    public async Task<IActionResult> Search(ProductSearchRequestDto query)
    {
        var result = await _valuService.SearchProduct(query);
        return Ok(result);
    }

    [HttpPost("product")]
    public async Task<IActionResult> GetProductDetails(ProductById query)
    {
        var result = await _valuService.GetProductDetails(query);
        return Ok(result);
    }
    [HttpPost("GetFeaturedProducts")]
    public async Task<IActionResult> GetFeaturedProducts(GetFeaturedProductDto query)
    {
        var result = await _valuService.GetFeaturedProducts(query);
        return Ok(result);
    }
    [HttpPost("GetStoreDetails")]
    public async Task<IActionResult> GetStoreDetails(GetStoreDto query)
    {
        var result = await _valuService.GetStoreDetails(query);
        return Ok(result);
    }
    [HttpGet("GetStores")]
    public async Task<IActionResult> GetStores()
    {
        var result = await _valuService.GetStores();
        return Ok(result);
    }
}
