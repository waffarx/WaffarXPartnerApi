using WaffarXPartnerApi.Domain.Entities.SqlEntities.PartnerEntities;

namespace WaffarXPartnerApi.Domain.RepositoryInterface.EntityFrameworkRepositoryInterface;
public interface IRefreshTokenRepository
{
    Task<RefreshToken> GetByTokenAsync(string token);
    Task<IEnumerable<RefreshToken>> GetByUserIdAsync(Guid userId);
    Task<bool> CreateAsync(RefreshToken refreshToken);
    Task<bool> UpdateAsync(RefreshToken refreshToken);
    Task<bool> RevokeAllUserTokensAsync(Guid userId);
}
