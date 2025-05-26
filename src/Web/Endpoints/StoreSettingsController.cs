using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WaffarXPartnerApi.Application.Common.DTOs.Dashboard.StoreSetting;
using WaffarXPartnerApi.Application.ServiceInterface.Dashboard;
using WaffarXPartnerApi.Domain.Constants;
using WaffarXPartnerApi.Domain.Enums;

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
    [RequiresPermission(AdminPageConstants.WhitelistedStores, AdminActionConstants.AddUpdateWhitelistStores)]
    public async Task<IActionResult> UpdateStoreSettingList(List<StoreSettingRequestDto> stores)
    {
        var response = await _storeSettingService.UpdateStoreSettingList(stores);
        return Ok(response);
    }

    /// <summary>
    /// Retrieves all store settings for the current client.
    /// </summary>
    /// <returns>GenericResponse containing a list of store settings.</returns>
    [HttpGet("getwhitelistedstores")]
    [RequiresPermission(AdminPageConstants.WhitelistedStores, AdminActionConstants.ListWhitelistStores)]

    public async Task<IActionResult> GetWhiteListedStores()
    {
        var response = await _storeSettingService.GetWhiteListedStores();

        return Ok(response);

    }

    [HttpGet("getallstore")]
    [RequiresPermission(AdminPageConstants.WhitelistedStores, AdminActionConstants.AddUpdateWhitelistStores)]
    public async Task<IActionResult> GetStoreLookUp()
    {
        var response = await _storeSettingService.GetStoreLookUp();

        return Ok(response);

    }


}
