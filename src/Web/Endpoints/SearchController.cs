using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WaffarXPartnerApi.Application.Common.DTOs.Valu.ValuRequestDto.ProductSearchRequest;
using WaffarXPartnerApi.Application.ServiceInterface.Dashboard;
using WaffarXPartnerApi.Domain.Enums;

namespace WaffarXPartnerApi.Web.Endpoints;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class SearchController : ControllerBase
{
    private readonly ISearchService _searchService;
    public SearchController(ISearchService searchService)
    {
        _searchService = searchService; 
    }

    [HttpPost("productsearch")]
    [RequiresPermission(nameof(AdminPageEnum.FeaturedProducts), nameof(AdminActionEnum.SearchProductWithStoreAndTerm))]
    public async Task<IActionResult> SearchProduct(ProductSearchRequestDto searchDto)
    {
        var response = await _searchService.SearchProduct(searchDto);
        return Ok(response);

    }
}
