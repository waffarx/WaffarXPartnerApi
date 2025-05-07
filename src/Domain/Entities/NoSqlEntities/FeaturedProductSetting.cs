using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace WaffarXPartnerApi.Domain.Entities.NoSqlEnitities;

[BsonIgnoreExtraElements]
public class FeaturedProductSetting
{
    [BsonId]
    public ObjectId Id { get; set; }

    public string SettingType { get; set; }

    public ProductFeature Product { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public int CreatedBy { get; set; }

    public int UpdatedBy { get; set; }

    public int ClientApiId { get; set; }
}
