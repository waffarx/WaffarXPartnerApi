using System.Security.Cryptography;
using WaffarXPartnerApi.Application.Common.DTOs;
using WaffarXPartnerApi.Application.Common.DTOs.Dashboard.Client;
using WaffarXPartnerApi.Application.Common.DTOs.Helper;
using WaffarXPartnerApi.Application.Common.Models.SharedModels;
using WaffarXPartnerApi.Application.ServiceInterface.Shared;
using WaffarXPartnerApi.Domain.Entities.SqlEntities.WaffarXEntities;
using WaffarXPartnerApi.Domain.Models.SharedModels;
using WaffarXPartnerApi.Domain.RepositoryInterface.EntityFrameworkRepositoryInterface;

namespace WaffarXPartnerApi.Application.ServiceImplementation.Shared;
public class ClientService : IClientService
{
    private readonly IApiClientRepository _apiClientRepository;
    private readonly RandomGenerator RandomGenerator;
    public ClientService(IApiClientRepository apiClientRepository, RandomGenerator randomGenerator)
    {
        _apiClientRepository = apiClientRepository;
        RandomGenerator = randomGenerator;
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
    public async Task<GenericResponse<bool>> CreateUser(CreateClientInput input)
    {
        try
        {
            var hmac = new HMACSHA256();
            var key = Convert.ToBase64String(hmac.Key);
            var secret = RandomGenerator.RandomString(256);
            var clientdata = await _apiClientRepository.GetClientByName(input.ClientName); 

            if (clientdata != null)
            {
                return new GenericResponse<bool>()
                {
                    Data = false,
                    Errors = new List<string>() { "" },
                    Status = StaticValues.Error
                };
            }

            var apiClient = new ApiClient()
            {
                Id = Guid.NewGuid().ToString(),
                Clientkey = key,
                Secret = secret,
                ClientName = input.ClientName,
                Language = "en",
                FirstName = input.FirstName,
                LastName = input.LastName,
                IsActive = false,
                Email = input.Email,
                TokenType = "Bearer",
                ExpiresIn = 3600,

            };

            var client = await _apiClientRepository.CreateClient(apiClient);
            var result = new GenericResponse<bool>()
            {
                Data = true,    
                Errors = null,
                Status = StaticValues.Success
            };

            return result;
        }
        catch (Exception)
        {
            throw;
        }
    }
}
