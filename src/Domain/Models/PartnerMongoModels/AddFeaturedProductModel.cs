using MongoDB.Bson;

namespace WaffarXPartnerApi.Domain.Models.PartnerMongoModels;
public class AddFeaturedProductModel
{
    public ObjectId ProductId { get; set; }
    public string StoreId { get; set; }
    public int StoreIdInt { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public int UserId { get; set; }
    public int ClientApiId { get; set; }
    public int ProductRank { get; set; }
}
