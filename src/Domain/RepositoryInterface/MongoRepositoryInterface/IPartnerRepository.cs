using WaffarXPartnerApi.Domain.Entities.NoSqlEnitities;
using WaffarXPartnerApi.Domain.Models.PartnerMongoModels;

namespace WaffarXPartnerApi.Domain.RepositoryInterface.MongoRepositoryInterface;
public interface IPartnerRepository
{
    Task<PaginationResultModel<List<FeaturedProductModel>>> GetFeaturedProducts(int clientApiId, List<int> WhiteListIds, bool isActive = false, int pageNumber = 1, int pageSize = 20);
    Task<List<int>> GetWhiteListStores(int apiClientId, List<int> disabledStores);
    Task<OfferSetting> GetOfferSetting(int clientApiId, string offerSettingId);
    Task<bool> AddUpdateOfferSetting(OfferSettingModel model);
}

public class OfferSettingModel
{
    public string Id { get; set; }
    public string OfferLookUpId { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public bool IsProductLevel { get; set; }
    public bool IsStoreLevel { get; set; }
    public List<int> StoreIds { get; set; }
    public List<string> ProductIds { get; set; }
    public int ClientApiId { get; set; }
    public int UserId { get; set; }


}
