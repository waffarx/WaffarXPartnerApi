using MongoDB.Bson;

namespace WaffarXPartnerApi.Domain.Models.PartnerMongoModels;
public class FeaturedProductModel
{
    public int StoreId { get; set; }
    public ObjectId ProductId { get; set; }
    public int ProductRank { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
}
