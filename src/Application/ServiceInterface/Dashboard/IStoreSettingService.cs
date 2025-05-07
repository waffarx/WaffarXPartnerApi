using WaffarXPartnerApi.Application.Common.DTOs.Dashboard;
using WaffarXPartnerApi.Application.Common.DTOs.Valu.SharedModels;
using WaffarXPartnerApi.Application.Common.Models.SharedModels;

namespace WaffarXPartnerApi.Application.ServiceInterface.Dashboard;
public interface IStoreSettingService
{
    Task<GenericResponse<bool>> UpdateStoreSettingList(List<StoreSettingRequestDto> stores);
    Task<GenericResponse<List<WhiteListedStoreResonseDto>>> GetWhiteListedStores();
    Task<GenericResponse<List<StoreLookUpResponseDto>>> GetStoreLookUp();
}
