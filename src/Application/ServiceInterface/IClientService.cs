using WaffarXPartnerApi.Application.Common.DTOs;

namespace WaffarXPartnerApi.Application.ServiceInterface;
public interface IClientService
{
    Task<ApiClientDto> GetUser(string Id);
}
