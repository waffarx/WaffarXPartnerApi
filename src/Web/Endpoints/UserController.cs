using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WaffarXPartnerApi.Application.Common.DTOs.Dashboard.Team.CreateTeam;
using WaffarXPartnerApi.Application.Common.DTOs.Dashboard.Team.UpdateTeam;
using WaffarXPartnerApi.Application.Common.DTOs.Dashboard.User;
using WaffarXPartnerApi.Application.Common.DTOs.Dashboard.User.AssignUserToTeam;
using WaffarXPartnerApi.Application.Common.DTOs.Dashboard.User.ResetPassword;
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

    [HttpPost("searchuser")]
    public async Task<IActionResult> SearchUser(UserSearchRequestDto request)
    {
        var response = await _userService.SearchForUser(request);
        return Ok(response);
    }

    [HttpGet("userdetails/{userId}")]
    public async Task<IActionResult> GetUserDetails(string userId)
    {
        var response = await _userService.GetUserDetails(userId);
        return Ok(response);
    }
    #region Team Endpoints

    /// <summary>
    /// Creates a new team.
    /// </summary>
    /// <param name="dto">The team creation data.</param>
    /// <returns>GenericResponse indicating success or failure.</returns>
    [HttpPost("createteam")]
    public async Task<IActionResult> CreateTeam(CreateTeamDto dto)
    {
        var response = await _userService.CreateTeamAsync(dto);
        return Ok(response);
    }

    /// <summary>
    /// Updates an existing team.
    /// </summary>
    /// <param name="dto">The team update data.</param>
    /// <returns>GenericResponse indicating success or failure.</returns>
    [HttpPost("updateteam")]
    public async Task<IActionResult> UpdateTeam(UpdateTeamDto dto)
    {
        var response = await _userService.UpdateTeamAsync(dto);
        return Ok(response);
    }

    /// <summary>
    /// Deletes a team by its ID.
    /// </summary>
    /// <param name="id">The ID of the team to delete.</param>
    /// <returns>GenericResponse indicating success or failure.</returns>
    [HttpDelete("deleteteam/{id:guid}")]
    public async Task<IActionResult> DeleteTeam(Guid id)
    {
        var response = await _userService.DeleteTeamAsync(id);
        return Ok(response);
    }

    /// <summary>
    /// Retrieves all teams for the current client API ID.
    /// </summary>
    /// <param name="clientApiId">The client API ID.</param>
    /// <returns>GenericResponse containing a list of teams.</returns>
    [HttpGet("allteam")]
    public async Task<IActionResult> GetAllTeams()
    {
        var response = await _userService.GetAllTeamsAsync();
        return Ok(response);
    }

    /// <summary>
    /// Retrieves the details of a specific team by its ID.
    /// </summary>
    /// <param name="id">The ID of the team.</param>
    /// <returns>GenericResponse containing the team details.</returns>
    [HttpGet("teamdetails/{id:guid}")]
    public async Task<IActionResult> GetTeamDetails(Guid id)
    {
        var response = await _userService.GetTeamDetailsAsync(id);
        return Ok(response);
    }
    [HttpPost("addupdateusertoteam")]
    public async Task<IActionResult> AddUserToTeam(AssignUserToTeamRequestDto model)
    {
        var response = await _userService.AddUserToTeamAsync(model);
        return Ok(response);
    }
    #endregion

}
