using MongoDB.Bson;

namespace WaffarXPartnerApi.Domain.Models.PartnerMongoModels.FeaturedProducts;
public class FeaturedProductsBase
{
    public ObjectId ProductId { get; set; }
    public int UserId { get; set; }
    public int ClientApiId { get; set; }
    public int ProductRank { get; set; }
}
