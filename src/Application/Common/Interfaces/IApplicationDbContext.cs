using WaffarXPartnerApi.Domain.Entities;

namespace WaffarXPartnerApi.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
