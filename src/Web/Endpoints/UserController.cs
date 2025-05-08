using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WaffarXPartnerApi.Application.ServiceInterface.Dashboard;

namespace WaffarXPartnerApi.API.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    /// <summary>
    /// Logs out the current user by invalidating their refresh tokens.
    /// </summary>
    /// <returns>GenericResponse indicating success or failure.</returns>
    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        var response = await _userService.Logout();

        return Ok(response);

    }

    /// <summary>
    /// Resets the password for the current user.
    /// </summary>
    /// <param name="model">The reset password request data.</param>
    /// <returns>GenericResponse indicating success or failure.</returns>
    [HttpPost("resetpassword")]
    public async Task<IActionResult> ResetPassword(ResetPasswordRequestDto model)
    {

        var response = await _userService.ResetPasswordAsync(model);
        return Ok(response);

    }
}
