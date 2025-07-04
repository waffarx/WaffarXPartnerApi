﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WaffarXPartnerApi.Application.Common.DTOs.Valu.ValuRequestDto.GetStoreBrands;
using WaffarXPartnerApi.Application.Common.DTOs.Valu.ValuRequestDto.ProductSearchRequest;
using WaffarXPartnerApi.Application.ServiceInterface.Dashboard;
using WaffarXPartnerApi.Domain.Constants;

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
    [RequiresPermission(AdminPageConstants.FeaturedProducts, AdminActionConstants.SearchProductWithStoreAndTerm)]
    public async Task<IActionResult> SearchProduct(ProductSearchRequestDto searchDto)
    {
        var response = await _searchService.SearchProduct(searchDto);
        return Ok(response);

    }

    [HttpPost("storebrandsearch")]
    [RequiresPermission(AdminPageConstants.FeaturedProducts, AdminActionConstants.SearchStoreBrands)]
    public async Task<IActionResult> BrandSearch(GetStoreBrandsDto brandsDto)
    {
        var response = await _searchService.SearchBrandsByStore(brandsDto);
        return Ok(response);

    }
}
