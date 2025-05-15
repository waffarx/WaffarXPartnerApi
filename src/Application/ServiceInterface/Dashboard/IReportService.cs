using WaffarXPartnerApi.Application.Common.DTOs.Dashboard.Postback;
using WaffarXPartnerApi.Application.Common.DTOs.Dashboard.Report;
using WaffarXPartnerApi.Application.Common.Models.SharedModels;

namespace WaffarXPartnerApi.Application.ServiceInterface.Dashboard;
public interface IReportService
{
    Task<GenericResponse<bool>> AddOrUpdatePostback(PostbackDto postbackDto);
    Task<GenericResponse<string>> GetPartnerPostbackUrl();
    Task<GenericResponse<GetParterOrderStatisticsDto>> GetPartnerOrdersStatistics();
}
