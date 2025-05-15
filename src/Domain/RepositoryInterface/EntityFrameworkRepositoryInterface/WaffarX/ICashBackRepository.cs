using WaffarXPartnerApi.Domain.Models.PartnerSqlModels;

namespace WaffarXPartnerApi.Domain.RepositoryInterface.EntityFrameworkRepositoryInterface.WaffarX;
public interface ICashBackRepository
{
    Task<GetParterOrderStatisticsModel> GetParterOrderStatistics(long UserId);
}
