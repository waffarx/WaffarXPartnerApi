using WaffarXPartnerApi.Domain.Entities.SqlEntities.PartnerEntities;

namespace WaffarXPartnerApi.Domain.RepositoryInterface.EntityFrameworkRepositoryInterface.Partner;
public interface IPartnerPostbackRepository
{
    Task<PartnerPostback> CreateAsync(PartnerPostback partnerPostback);
    Task<bool> UpdateAsync(PartnerPostback partnerPostback);
    Task<PartnerPostback> GetByClientIdAsync(int clientApiId);
}
