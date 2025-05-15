using Microsoft.EntityFrameworkCore;
using WaffarXPartnerApi.Domain.Models.PartnerSqlModels;
using WaffarXPartnerApi.Domain.RepositoryInterface.EntityFrameworkRepositoryInterface.WaffarX;
using WaffarXPartnerApi.Infrastructure.Data;

namespace WaffarXPartnerApi.Infrastructure.RepositoryImplementation.EntityFrameworkRepository.WaffarX;
public class CashBackRepository : ICashBackRepository
{
    private readonly WaffarXContext _context;   
    public CashBackRepository(WaffarXContext context)
    {
        _context = context; 
    }
    public async Task<GetParterOrderStatisticsModel> GetParterOrderStatistics(long UserId)
    {
        try
        {
            GetParterOrderStatisticsModel model = await _context.CashBacks.Where(x => x.UserId == UserId)
                .GroupBy(x => new { x.UserId})
                .Select(cb => new GetParterOrderStatisticsModel
                {
                    OrdersCount = cb.Count(),
                    OrdersGmv = cb.Sum(x => x.OrderValueEgp ?? 0)
                }).FirstOrDefaultAsync();
            model.ExitClickCount = await _context.ExitClicks.Where(x => x.UserId == UserId).CountAsync();
            return model;   
        }
        catch (Exception)
        {
            throw;
        }
    }
}
