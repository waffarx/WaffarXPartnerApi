using Microsoft.EntityFrameworkCore;
using WaffarXPartnerApi.Application.Common.Interfaces;
using WaffarXPartnerApi.Domain.Entities.SqlEntities.PartnerEntities;
using WaffarXPartnerApi.Domain.RepositoryInterface.EntityFrameworkRepositoryInterface.Partner;
using WaffarXPartnerApi.Infrastructure.Data;

namespace WaffarXPartnerApi.Infrastructure.RepositoryImplementation.EntityFrameworkRepository.Partner;
public class PartnerPostbackRepository : IPartnerPostbackRepository
{
    private readonly WaffarXPartnerDbContext _context;
    public PartnerPostbackRepository(WaffarXPartnerDbContext context)
    {
        _context = context;
    }
    public async Task<PartnerPostback> CreateAsync(PartnerPostback partnerPostback)
    {
        try
        {
            _context.PartnerPostbacks.Add(partnerPostback);
            await _context.SaveChangesAsync();
            return partnerPostback;
        }
        catch (Exception)
        {
            throw;
        }
    }
    public async Task<PartnerPostback> GetByClientIdAsync(int clientApiId)
    {
        return await _context.PartnerPostbacks.FirstOrDefaultAsync(p => p.ClientApiId == clientApiId);    
    }
    public async Task<bool> UpdateAsync(PartnerPostback partnerPostback)
    {
        try
        {
            var updated = 0;
            if (partnerPostback != null)
            {
                partnerPostback.UpdatedAt = DateTime.Now;
                _context.Entry(partnerPostback).State = EntityState.Modified;
                updated = await _context.SaveChangesAsync();
            }
            return updated > 0;
        }
        catch (Exception)
        {

            throw;
        }
    }
}
