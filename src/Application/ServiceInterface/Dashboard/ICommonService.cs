using WaffarXPartnerApi.Application.Common.DTOs.Dashboard.Page;
using WaffarXPartnerApi.Application.Common.DTOs.Dashboard.User;
using WaffarXPartnerApi.Application.Common.Models.SharedModels;

namespace WaffarXPartnerApi.Application.ServiceInterface.Dashboard;
public interface ICommonService
{
    Task<GenericResponse<List<PageDetailDto>>> GetPages();
}
