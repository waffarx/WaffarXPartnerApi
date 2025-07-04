﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WaffarXPartnerApi.Application.Common.DTOs.Valu.Login;
using WaffarXPartnerApi.Application.Common.DTOs.Valu.Register;
using WaffarXPartnerApi.Application.ServiceInterface.Dashboard;
using WaffarXPartnerApi.Domain.Constants;

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
        var result = await _authService.LoginAsync(request.Email, request.Password);
        return Ok(result);
    }

    [Authorize]
    [HttpPost("register")]
    [RequiresPermission(AdminPageConstants.AddMember, AdminActionConstants.AddMember)]
    public async Task<IActionResult> Register(RegisterRequestDto request)
    {
        var result = await _authService.RegisterAsync(request);
        return Ok(result);
    }

    [HttpPost("refreshtoken")]
    public async Task<IActionResult> RefreshToken(TokenDto model)
    {
        var result = await _authService.RefreshTokenAsync(model.Token);
        return Ok(result);
    }

    [Authorize]
    [HttpPost("setactivateuser")]
    [RequiresPermission(AdminPageConstants.Members, AdminActionConstants.DeactivateMember)]
    public async Task<IActionResult> DeactivateUser(string userId)
    {
        var result = await _authService.DeactivateUser(userId);
        return Ok(result);
    }
}

public class TokenDto
{
    public required string Token { get; set; }
}
