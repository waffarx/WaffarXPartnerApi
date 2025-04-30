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
    [CompressResponse]  
    public async Task<IActionResult> Search(ProductSearchRequestDto query)
    {
        var result = await _valuService.SearchProduct(query);
        return Ok(result);
    }

    [HttpPost("GetFeaturedProducts")]
    public async Task<IActionResult> GetFeaturedProducts(GetFeaturedProductDto query)
    {
        var result = await _valuService.GetFeaturedProducts(query);
        return Ok(result);
    }

    [HttpPost("GetStores")]
    public async Task<IActionResult> GetStores(GetStoresRequestDto model)
    {
        var result = await _valuService.GetStores(model);
        return Ok(result);
    }

    [HttpGet("product")]
    [HttpGet("product/{id}")]
    public async Task<IActionResult> GetProductDetails(string id)
    {
        var result = await _valuService.GetProductDetails(id);
        return Ok(result);
    }
    [HttpGet("GetStoreDetails")]
    [HttpGet("GetStoreDetails/{id}")]
    public async Task<IActionResult> GetStoreDetails(Guid id)
    {
        var result = await _valuService.GetStoreDetails(id);
        return Ok(result);
    }
    [HttpGet("ShoppingTrip/{section}/{storeId}/{productId?}")]
    public async Task<IActionResult> ShoppingTrip(string section, Guid storeId, string productId = "")
    {
        var result = await _valuService.CreateExitClick(section, storeId, productId);    
        return Ok(result);
    }

}
