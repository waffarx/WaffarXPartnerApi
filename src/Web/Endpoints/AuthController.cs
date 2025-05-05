using Microsoft.AspNetCore.Mvc;
using WaffarXPartnerApi.Application.Common.DTOs;
using WaffarXPartnerApi.Application.ServiceInterface;
using WaffarXPartnerApi.Domain.Entities.SqlEntities.PartnerEntities;

namespace WaffarXPartnerApi.Web.Endpoints;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }
    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequestDto request)
    {
        var result = await _authService.LoginAsync(request.Username, request.Password);
        if (result.Success)
        {
            return Ok(result);
        }
        return BadRequest(result.Message);
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterRequestDto request)
    {
        var user = new User
        {
            Username = request.Username,
            Email = request.Email,
            FirstName = request.FirstName,
            LastName = request.LastName,
            ClientApiId = request.ClientApiId,
            
            
        };
        var result = await _authService.RegisterAsync(user, request.Password);
        if (result.Success)
        {
            return Ok(result);
        }
        return BadRequest(result.Message);
    }
    [HttpPost("refreshtoken")]
    public async Task<IActionResult> RefreshToken(TokenDto model)
    {
        var result = await _authService.RefreshTokenAsync(model.Token);
        if (result.Success)
        {
            return Ok(result);
        }
        return BadRequest(result.Message);
    }
    [HttpPost("revoketoken")]
    public async Task<IActionResult> RevokeToken(TokenDto model)
    {
        var result = await _authService.RevokeTokenAsync(model.Token);
        if (result)
        {
            return Ok(new { Message = "Token revoked successfully." });
        }
        return BadRequest("Failed to revoke token.");
    }
    [HttpPost("validatetoken")]
    public async Task<IActionResult> ValidateToken(TokenDto model)
    {
        var result = await _authService.ValidateJwtTokenAsync(model.Token);
        if (result.Success)
        {
            return Ok(result);
        }
        return BadRequest(result.Message);
    }


}

public class TokenDto
{
    public required string Token { get; set; }
}
