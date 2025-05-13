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
        catch (Exception)
        {
            // Log the exception
            throw;
        }

    }

    public async Task<long> GetUserIdByClient(string Id)
    {
        try
        {
            return await _context.AppUsersClients.Where(ac => ac.UserToken == Id).Select(x => x.UserId).FirstOrDefaultAsync();
        }
        catch (Exception)
        {
            // Log the exception
            throw;
        }

    }

    public async Task<int> GetClientIdByGuid(string Id)
    {
        try
        {
            return await _context.ApiClients.Where(ac => ac.Id == Id && ac.IsActive == true).Select(c => c.ClientId).FirstOrDefaultAsync();
        }
        catch (Exception)
        {
            // Log the exception
            throw;
        }

    }

    public async Task<Guid> GetClientGuidById(int Id)
    {
        try
        {
            return await _context.ApiClients
                .Where(ac => ac.ClientId == Id && ac.IsActive == true)
                .Select(c => Guid.Parse(c.Id)) // Convert string to Guid  
                .FirstOrDefaultAsync();
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<ApiClient> GetClientByName(string name)
    {
        try
        {
            return await _context.ApiClients.FirstOrDefaultAsync(x => x.ClientName == name);
        }
        catch (Exception)
        {
            throw;
        }

    }

    public async Task<bool> CreateClient(ApiClient entity)
    {
        try
        {
            _context.ApiClients.Add(entity);
            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception)
        {
            throw;
        }

    }
}
