using Microsoft.AspNetCore.Mvc;
using WaffarXPartnerApi.Application.Common.DTOs.Valu.ValuRequestDto.GetFeaturedByStoreRequest;
using WaffarXPartnerApi.Application.Common.DTOs.Valu.ValuRequestDto.GetFeaturedProductRequest;
using WaffarXPartnerApi.Application.Common.DTOs.Valu.ValuRequestDto.GetProductsOffersRequest;
using WaffarXPartnerApi.Application.Common.DTOs.Valu.ValuRequestDto.GetStoreBrands;
using WaffarXPartnerApi.Application.Common.DTOs.Valu.ValuRequestDto.GetStoresRequest;
using WaffarXPartnerApi.Application.Common.DTOs.Valu.ValuRequestDto.GetStoresWithProductsRequest;
using WaffarXPartnerApi.Application.Common.DTOs.Valu.ValuRequestDto.ProductSearchRequest;
using WaffarXPartnerApi.Application.Common.DTOs.Valu.ValuRequestDto.StoreProductSearchRequest;
using WaffarXPartnerApi.Application.Common.DTOs.Valu.ValuRequestDto.StoreSearchWithFiltersRequest;
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

    [HttpPost("getfeaturedproducts")]
    public async Task<IActionResult> GetFeaturedProducts(GetFeaturedProductByStoreDto query)
    {
        var result = await _valuService.GetFeaturedProducts(query);
        return Ok(result);
    }

    [HttpGet("product/{id}")]
    public async Task<IActionResult> GetProductDetails(string id)
    {
        var result = await _valuService.GetProductDetails(id);
        return Ok(result);
    }

    [HttpGet("getstoredetails/{id}")]
    public async Task<IActionResult> GetStoreDetails(Guid id)
    {
        var result = await _valuService.GetStoreDetails(id);
        return Ok(result);
    }

    [HttpGet("shoppingtrip/{section}/{storeId}/{productId?}")]
    public async Task<IActionResult> ShoppingTrip(string section, Guid storeId, string productId = "", string uId = ""
        ,string subId = "", string variant = "")
    {
        var result = await _valuService.CreateExitClick(section, storeId, productId, uId, subId, variant);
        return Ok(result);
    }

    [HttpPost("storesearch")]
    [CompressResponse]
    public async Task<IActionResult> StoreSearch(StoreProductSearchRequestDto query)
    {
        var result = await _valuService.StoreSearchProduct(query);
        return Ok(result);
    }

    [HttpGet("getstorecategories/{id}")]
    public async Task<IActionResult> GetStoredCategories(Guid id)
    {
        var result = await _valuService.GetStoreCategories(id);
        return Ok(result);
    }

    [HttpPost("getstores")]
    public async Task<IActionResult> StoresWithOffers(GetStoresRequestDto model)
    {
        var result = await _valuService.GetStoresWithOffers(model);
        return Ok(result);
    }

    [HttpPost("getstoreswithproducts")]
    public async Task<IActionResult> Getstoreswithproducts(GetStoresWithProductsDto model)
    {
        var result = await _valuService.GetStoresWithProducts(model);
        return Ok(result);
    }

    [HttpPost("getoffersproducts")]
    public async Task<IActionResult> GetProductsByOffers(GetProductsByOffersDto model)
    {
        var result = await _valuService.GetProductsByOffers(model);
        return Ok(result);
    }

    [HttpPost("getofferstores")]
    public async Task<IActionResult> OffersStores(GetProductsByOffersDto model)
    {
        var result = await _valuService.GetStoresByActiveOffers(model);
        return Ok(result);
    }

    [HttpPost("storebrandsearch")]
    public async Task<IActionResult> GetBrandsByStore(GetStoreBrandsDto brandsDto)
    {
        var result = await _valuService.SearchBrandsByStore(brandsDto);
        return Ok(result);
    }

    [HttpPost("storesearchwithfilters")]
    [CompressResponse]
    public async Task<IActionResult> SearchByStore(StoreSearchWithFiltersDto query)
    {
        var result = await _valuService.SearchByStoreWithFilter(query);
        return Ok(result);
    }
}
