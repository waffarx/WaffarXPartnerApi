using Microsoft.AspNetCore.Mvc;
using WaffarXPartnerApi.Application.Common.DTOs.Dashboard.Client;
using WaffarXPartnerApi.Application.ServiceInterface.Shared;

namespace WaffarXPartnerApi.Web.Endpoints;

[ApiController]
[Route("api/[controller]")]
public class ClientController : ControllerBase
{
    private readonly IClientService _clientService;

    public ClientController(IClientService clientService)
    {
        _clientService = clientService;
    }

    [HttpPost("createclient")]
    public async Task<IActionResult> Login(CreateClientInput request)
    {
        var result = await _clientService.CreateUser(request);
        return Ok(result);

    }
}
