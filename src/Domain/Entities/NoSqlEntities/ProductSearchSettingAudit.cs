using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace WaffarXPartnerApi.Domain.Entities.NoSqlEnitities;
public class ProductSearchSettingAudit
{
    [BsonId]
    public ObjectId Id { get; set; }

    public int CreatedBy { get; set; }

    public DateTime CreatedAt { get; set; }

    public int ClientApiId { get; set; }

    [BsonElement("ProductSearchSetting")]
    public ProductSearchSetting OriginalDocument { get; set; }
}
