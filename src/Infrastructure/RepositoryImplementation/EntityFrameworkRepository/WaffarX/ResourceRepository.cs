using Microsoft.EntityFrameworkCore;
using WaffarXPartnerApi.Domain.Models.SharedModels;
using WaffarXPartnerApi.Domain.RepositoryInterface.EntityFrameworkRepositoryInterface.WaffarX;
using WaffarXPartnerApi.Infrastructure.Data;

namespace WaffarXPartnerApi.Infrastructure.RepositoryImplementation.EntityFrameworkRepository.WaffarX;
public class ResourceRepository : IResourceRepository
{
    private readonly WaffarXContext _context;
    public ResourceRepository(WaffarXContext context)
    {
        _context = context;
    }

    public async Task<List<ResourcesModel>> GetAllResources()
    {
        try
        {
            return await _context.Resources
                                .Select(rs => new ResourcesModel
                                {
                                    Id = rs.Id,
                                    Key = rs.Key,
                                    ValueEn = rs.ValueEn,
                                    ValueAr = rs.ValueAr,
                                    ValueSa = rs.ValueSa,
                                    AllValueAr = rs.ValueAr
                                }).ToListAsync();
        }
        catch (Exception)
        {
            throw;
        }
    }
}
