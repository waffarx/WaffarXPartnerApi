using WaffarXPartnerApi.Domain.RepositoryInterface.MongoRepositoryInterface;

namespace WaffarXPartnerApi.Domain.Models.PartnerMongoModels;
public class UpdateWhiteListStoreRequestModel
{
    public List<WhitelistStoreToUpdate> StoreList { get; set; }
    public int ClientApiId { get; set; }
    public int UserId { get; set; }
}
