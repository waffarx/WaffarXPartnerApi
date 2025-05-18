using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WaffarXPartnerApi.Application.Common.DTOs.Dashboard.Products.AddFeaturedProduct;
using WaffarXPartnerApi.Application.Common.DTOs.Dashboard.Products.DeleteFeatured;
using WaffarXPartnerApi.Application.Common.DTOs.Dashboard.Products.FeaturedProducts.GetFeaturedProduct;
using WaffarXPartnerApi.Application.Common.DTOs.Dashboard.Products.RankFeaturedProducts;
using WaffarXPartnerApi.Application.Common.DTOs.Dashboard.Products.UpdateFeatured;
using WaffarXPartnerApi.Application.ServiceInterface.Dashboard;
using WaffarXPartnerApi.Domain.Enums;

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

    [HttpPost("getfeaturedproducts")]
    [RequiresPermission(nameof(AdminPageEnum.FeaturedProducts), nameof(AdminActionEnum.ListFeaturedProducts))]
    public async Task<IActionResult> GetFeaturedProducts(GetPartnerFeaturedProductDto featuredProductDto)
    {
        var response = await _productSettingService.GetFeaturedProducts(featuredProductDto);
        return Ok(response);

    }

    [HttpPost("deletefeaturedproducts")]
    [RequiresPermission(nameof(AdminPageEnum.FeaturedProducts), nameof(AdminActionEnum.DeleteFeaturedProduct))]

    public async Task<IActionResult> DeleteFeaturedProducts(DeleteFeaturedProductDto featuredProductDto)
    {
        var response = await _productSettingService.DeleteFeaturedProduct(featuredProductDto);
        return Ok(response);

    }

    [HttpPost("updatefeaturedproduct")]
    [RequiresPermission(nameof(AdminPageEnum.FeaturedProducts), nameof(AdminActionEnum.UpdateFeaturedProduct))]

    public async Task<IActionResult> UpdateFeaturedProduct(UpdateFeaturedProductDto featuredProductDto)
    {
        var response = await _productSettingService.UpdateFeaturedProduct(featuredProductDto);
        return Ok(response);

    }

    [HttpPost("addfeaturedproducts")]
    [RequiresPermission(nameof(AdminPageEnum.FeaturedProducts), nameof(AdminActionEnum.AddFeaturedProducts))]

    public async Task<IActionResult> UpdateFeaturedProduct(List<AddFeaturedProductDto> products)
    {
        var response = await _productSettingService.AddFeaturedProductList(products);
        return Ok(response);

    }
    
    [HttpPost("saveproductranks")]
    [RequiresPermission(nameof(AdminPageEnum.RankFeaturedProducts), nameof(AdminActionEnum.RankFeaturedProducts))]

    public async Task<IActionResult> UpdateProductRanks(List<RankProductsDto> products)
    {
        var response = await _productSettingService.SaveFeaturedProductRank(products);
        return Ok(response);

    }
}
