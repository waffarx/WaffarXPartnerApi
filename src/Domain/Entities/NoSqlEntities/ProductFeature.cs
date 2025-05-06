using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace WaffarXPartnerApi.Domain.Entities.NoSqlEnitities;
public class ProductFeature
{
    [BsonId]

    public ObjectId ProductId { get; set; }
    public int StoreId { get; set; }
    public int ProductRank { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }
}
