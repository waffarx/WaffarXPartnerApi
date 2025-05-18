using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WaffarXPartnerApi.Application.Common.DTOs.Dashboard.Offers.OfferLookUp;
using WaffarXPartnerApi.Application.Common.DTOs.Dashboard.Offers.OfferSetting;
using WaffarXPartnerApi.Application.ServiceInterface.Dashboard;
using WaffarXPartnerApi.Domain.Enums;

namespace WaffarXPartnerApi.API.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class OfferSettingController : ControllerBase
{
    private readonly IOfferSettingService _offerSettingService;

    public OfferSettingController(IOfferSettingService offerSettingService)
    {
        _offerSettingService = offerSettingService;
    }

    /// <summary>
    /// Adds or updates an offer setting.
    /// </summary>
    /// <param name="model">The offer setting request data.</param>
    /// <returns>GenericResponse indicating success or failure.</returns>
    [HttpPost("addoffer")]
    [RequiresPermission(nameof(AdminPageEnum.AddEditOffer), nameof(AdminActionEnum.AssignOfferToProductsOrStores))]
    public async Task<IActionResult> AddOffer(OfferSettingRequestDto model)
    {
        var response = await _offerSettingService.AddOrUpdateOffer(model);
        return Ok(response);

    }
    [HttpPost("updateoffer")]
    [RequiresPermission(nameof(AdminPageEnum.AddEditOffer), nameof(AdminActionEnum.UpdateOfferProductsOrStoresListing))]
    public async Task<IActionResult> UpdateOffer(OfferSettingRequestDto model)
    {
        var response = await _offerSettingService.AddOrUpdateOffer(model);
        return Ok(response);

    }
    /// <summary>
    /// Retrieves all offers for the current client.
    /// </summary>
    /// <returns>GenericResponse containing a list of offers.</returns>
    /// 
    [HttpGet("getoffer")]
    [RequiresPermission(nameof(AdminPageEnum.OffersListing), nameof(AdminActionEnum.ListOffers))]
    public async Task<IActionResult> GetOffers()
    {
        var response = await _offerSettingService.GetOffers();
        return Ok(response);

    }
    /// <summary>
    /// Retrieves the details of a specific offer.
    /// </summary>
    /// <param name="model">The offer detail request data.</param>
    /// <returns>GenericResponse containing the offer details.</returns>

    [HttpGet("getofferdetail/{id}")]
    [RequiresPermission(nameof(AdminPageEnum.OffersListing), nameof(AdminActionEnum.ListOffers))]
    public async Task<IActionResult> GetOfferDetails(string id)
    {
        var response = await _offerSettingService.GetOfferDetails(id);
        return Ok(response);
    }

    /// <summary>
    /// Adds or updates an offer lookup.
    /// </summary>
    /// <param name="model">The offer lookup request data.</param>
    /// <returns>GenericResponse indicating success or failure.</returns>
    [HttpPost("addlookup")]
    [RequiresPermission(nameof(AdminPageEnum.OffersLookups), nameof(AdminActionEnum.CreateOffer))]
    public async Task<IActionResult> AddOfferLookUp(OfferLookUpRequestDto model)
    {
        var response = await _offerSettingService.AddOrUpdateOfferLookUp(model);
        return Ok(response);
    }

    [HttpPost("updatelookup")]
    [RequiresPermission(nameof(AdminPageEnum.OffersLookups), nameof(AdminActionEnum.UpdateOffer))]
    public async Task<IActionResult> UpdateOfferLookUp(OfferLookUpRequestDto model)
    {
        var response = await _offerSettingService.AddOrUpdateOfferLookUp(model);
        return Ok(response);
    }
    /// <summary>
    /// Retrieves all offer lookups for the current client.
    /// </summary>
    /// <returns>GenericResponse containing a list of offer lookups.</returns>
    [HttpGet("getofferlookup")]
    [RequiresPermission(nameof(AdminPageEnum.OffersLookups), nameof(AdminActionEnum.ListAllOffers))]
    public async Task<IActionResult> GetOffersLookup()
    {
        var response = await _offerSettingService.GetOffersLookup();
        return Ok(response);
    }
}
