using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WaffarXPartnerApi.Application.Common.DTOs.Dashboard;
using WaffarXPartnerApi.Application.Common.Models.SharedModels;
using WaffarXPartnerApi.Application.ServiceInterface.Dashboard;

namespace WaffarXPartnerApi.API.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]

public class StoreSettingsController : ControllerBase
{
    private readonly IStoreSettingService _storeSettingService;

    public StoreSettingsController(IStoreSettingService storeSettingService)
    {
        _storeSettingService = storeSettingService;
    }

    /// <summary>
    /// Updates the store settings for the current client.
    /// </summary>
    /// <param name="stores">List of store settings to update.</param>
    /// <returns>GenericResponse indicating success or failure.</returns>
    [HttpPost("update")]
    public async Task<IActionResult> UpdateStoreSettingList(List<StoreSettingRequestDto> stores)
    {
        if (stores == null || !stores.Any())
        {
            return BadRequest(new GenericResponse<bool>
            {
                Data = false,
                Message = "Invalid input. The list of stores cannot be empty."
            });
        }

        var response = await _storeSettingService.UpdateStoreSettingList(stores);

        if (response.Data)
        {
            return Ok(response);
        }

        return StatusCode(500, response);
    }

    /// <summary>
    /// Retrieves all store settings for the current client.
    /// </summary>
    /// <returns>GenericResponse containing a list of store settings.</returns>
    [HttpGet("getwhitelistedstores")]
    public async Task<IActionResult> GetWhiteListedStores()
    {
        var response = await _storeSettingService.GetWhiteListedStores();

        return Ok(response);

    }

    [HttpGet("getallstore")]
    public async Task<IActionResult> GetStoreLookUp()
    {
        var response = await _storeSettingService.GetStoreLookUp();

        return Ok(response);

    }
}
