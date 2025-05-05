using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WaffarXPartnerApi.Application.Common.DTOs.Dashboard.Offers;
using WaffarXPartnerApi.Application.ServiceInterface.Dashboard;

namespace WaffarXPartnerApi.API.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class OfferSettingsController : ControllerBase
{
    private readonly IOfferSettingService _offerSettingService;

    public OfferSettingsController(IOfferSettingService offerSettingService)
    {
        _offerSettingService = offerSettingService;
    }

    /// <summary>
    /// Adds or updates an offer setting.
    /// </summary>
    /// <param name="model">The offer setting request data.</param>
    /// <returns>GenericResponse indicating success or failure.</returns>
    [HttpPost("addupdateoffer")]
    public async Task<IActionResult> AddOrUpdateOffer(OfferSettingRequestDto model)
    {
        var response = await _offerSettingService.AddOrUpdateOffer(model);
        return Ok(response);

    }
    /// <summary>
    /// Retrieves all offers for the current client.
    /// </summary>
    /// <returns>GenericResponse containing a list of offers.</returns>
    [HttpGet("getoffer")]
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
    [HttpPost("getofferdetail")]
    public async Task<IActionResult> GetOfferDetails(OfferDetailRequestDto model)
    {
        var response = await _offerSettingService.GetOfferDetails(model);
        return Ok(response);
    }

    /// <summary>
    /// Adds or updates an offer lookup.
    /// </summary>
    /// <param name="model">The offer lookup request data.</param>
    /// <returns>GenericResponse indicating success or failure.</returns>
    [HttpPost("addupdatelookup")]
    public async Task<IActionResult> AddOrUpdateOfferLookUp(OfferLookUpRequestDto model)
    {
        var response = await _offerSettingService.AddOrUpdateOfferLookUp(model);
        return Ok(response);
    }

    /// <summary>
    /// Retrieves all offer lookups for the current client.
    /// </summary>
    /// <returns>GenericResponse containing a list of offer lookups.</returns>
    [HttpGet("getofferlookup")]
    public async Task<IActionResult> GetOffersLookup()
    {
        var response = await _offerSettingService.GetOffersLookup();
        return Ok(response);
    }
}
