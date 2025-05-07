using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WaffarXPartnerApi.Application.Common.DTOs.Dashboard.Products.FeaturedProducts.GetFeaturedProduct;
using WaffarXPartnerApi.Application.ServiceImplementation.Dashboard;
using WaffarXPartnerApi.Application.ServiceInterface.Dashboard;

namespace WaffarXPartnerApi.Web.Endpoints;

[Authorize]
[ApiController]
[Route("api/[controller]")]

public class ProductSettingController : ControllerBase
{
    private readonly IProductSettingService _productSettingService;

    public ProductSettingController(IProductSettingService productSettingService)
    {
        _productSettingService = productSettingService;
    }


    [HttpGet("getfeaturedproducts")]
    public async Task<IActionResult> GetOffers(GetPartnerFeaturedProductDto featuredProductDto)
    {
        var response = await _productSettingService.GetFeaturedProducts(featuredProductDto);
        return Ok(response);

    }
}
