using Microsoft.EntityFrameworkCore;
using WaffarXPartnerApi.Domain.Entities.SqlEntities.WaffarXEntities;
using WaffarXPartnerApi.Domain.RepositoryInterface.EntityFrameworkRepositoryInterface;
using WaffarXPartnerApi.Infrastructure.Data;

namespace WaffarXPartnerApi.Infrastructure.RepositoryImplementation.EntityFrameworkRepository;
public class ApiClientRepository : IApiClientRepository
{
    private readonly WaffarXContext _context;

    public ApiClientRepository(WaffarXContext context)
    {
        _context = context;
    }

    public async Task<ApiClient> GetClientById(string Id)
    {
        try
        {
            return await _context.ApiClients.Where(ac => ac.Id == Id && ac.IsActive == true).FirstOrDefaultAsync();
        }
        catch(Exception)
        {
            // Log the exception
            throw;
        }

    }
}
