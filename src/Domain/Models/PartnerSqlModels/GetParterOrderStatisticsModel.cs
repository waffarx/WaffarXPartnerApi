
namespace WaffarXPartnerApi.Domain.Models.PartnerSqlModels;
public class GetParterOrderStatisticsModel
{
    public int OrdersCount { get; set; }
    public decimal OrdersGmv { get; set; }
    public int ExitClickCount { get; set; }
}
