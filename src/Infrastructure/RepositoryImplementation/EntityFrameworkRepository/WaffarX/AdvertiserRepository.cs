using Microsoft.EntityFrameworkCore;
using WaffarXPartnerApi.Domain.RepositoryInterface.EntityFrameworkRepositoryInterface.WaffarX;
using WaffarXPartnerApi.Infrastructure.Data;

namespace WaffarXPartnerApi.Infrastructure.RepositoryImplementation.EntityFrameworkRepository.WaffarX;
public class AdvertiserRepository : IAdvertiserRepository
{
    private readonly WaffarXContext _context;
    public AdvertiserRepository(WaffarXContext context)
    {
        _context = context;
    }
    public async Task<List<int>> GetStoreIds(List<Guid> Guids)
    {
        try
        {
            return await _context.Advertisers.Where(ad => Guids.Contains(ad.StoreGuid.Value) && ad.IsActive == true).Select(x => x.Id).ToListAsync();
        }
        catch (Exception)
        {
            throw;
        }
    }

}
