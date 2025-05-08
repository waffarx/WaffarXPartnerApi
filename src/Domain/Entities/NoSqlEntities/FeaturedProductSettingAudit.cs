using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace WaffarXPartnerApi.Domain.Entities.NoSqlEnitities;
public class FeaturedProductSettingAudit
{
    [BsonId]
    public ObjectId Id { get; set; }
    public int CreatedBy { get; set; }
    public DateTime CreatedAt { get; set; }
    public int ClientApiId { get; set; }
    public string Type { get; set; }
    [BsonElement("FeaturedProductSetting")]
    public FeaturedProductSetting OriginalDocument { get; set; }
}
