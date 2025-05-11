using MongoDB.Bson;

namespace WaffarXPartnerApi.Domain.Models.PartnerMongoModels.FeaturedProducts;
public class AddFeaturedProductModel : FeaturedProductsBase
{
    public string StoreId { get; set; }
    public int StoreIdInt { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
}
