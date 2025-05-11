using WaffarXPartnerApi.Application.Common.DTOs;

namespace WaffarXPartnerApi.Application.ServiceInterface.Shared;
public interface IClientService
{
    Task<ApiClientDto> GetUser(string Id);
}
