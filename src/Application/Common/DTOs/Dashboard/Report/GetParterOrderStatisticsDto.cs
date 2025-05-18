namespace WaffarXPartnerApi.Application.Common.DTOs.Dashboard.Report;
public class GetParterOrderStatisticsDto
{
    public int OrdersCount { get; set; }
    public decimal OrdersGmv { get; set; }
    public int ExitClickCount { get; set; }
}
