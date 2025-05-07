using MongoDB.Bson;
using WaffarXPartnerApi.Domain.Entities.NoSqlEnitities;
using WaffarXPartnerApi.Domain.Models.PartnerMongoModels;

namespace WaffarXPartnerApi.Domain.RepositoryInterface.MongoRepositoryInterface;
public interface IPartnerRepository
{
    Task<PaginationResultModel<List<FeaturedProductModel>>> GetFeaturedProducts(int clientApiId, List<int> WhiteListIds, bool isActive = false, int pageNumber = 1, int pageSize = 20);
    Task<List<int>> GetWhiteListStores(int apiClientId, List<int> disabledStores);
    Task<OfferSetting> GetOfferSetting(int clientApiId, string offerSettingId);
    Task<bool> AddUpdateOfferSetting(OfferSettingModel model);
    Task<bool> DeleteFeaturedProductById(ObjectId productId, int clientApiId, int userId);
}
