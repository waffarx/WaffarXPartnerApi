using WaffarXPartnerApi.Application.Common.DTOs;
using WaffarXPartnerApi.Application.ServiceInterface;
using WaffarXPartnerApi.Domain.RepositoryInterface.EntityFrameworkRepositoryInterface;

namespace WaffarXPartnerApi.Application.ServiceImplementation;
public class ClientService : IClientService
{
    private readonly IApiClientRepository _apiClientRepository;

    public ClientService(IApiClientRepository apiClientRepository)
    {
        _apiClientRepository = apiClientRepository;
    }

    public async Task<ApiClientDto> GetUser(string id)
    {
        try
        {
            var client = await _apiClientRepository.GetClientById(id);
            if (client == null)
            {
                return null;
            }
            var clientDto = new ApiClientDto
            {
                Id = client.Id,
                ClientId = client.ClientId,
                ClientName = client.ClientName,
                IsActive = client.IsActive,
                Secret = client.Secret,
                Clientkey = client.Clientkey,
            };
            return clientDto;
        }
        catch (Exception)
        {
            throw;
        }
    }
}
