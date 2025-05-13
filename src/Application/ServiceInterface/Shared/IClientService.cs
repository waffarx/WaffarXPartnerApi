using WaffarXPartnerApi.Application.Common.DTOs;
using WaffarXPartnerApi.Application.Common.DTOs.Dashboard.Client;
using WaffarXPartnerApi.Application.Common.Models.SharedModels;

namespace WaffarXPartnerApi.Application.ServiceInterface.Shared;
public interface IClientService
{
    Task<ApiClientDto> GetUser(string Id);
    Task<GenericResponse<bool>> CreateUser(CreateClientInput model);
}
