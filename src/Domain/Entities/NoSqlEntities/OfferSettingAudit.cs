using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace WaffarXPartnerApi.Domain.Entities.NoSqlEnitities;
public class OfferSettingAudit
{
    [BsonId]
    public ObjectId Id { get; set; }

    public int CreatedBy { get; set; }

    public DateTime CreatedAt { get; set; }

    public int ClientApiId { get; set; }

    [BsonElement("OfferSetting")]
    public OfferSetting OriginalDocument { get; set; }
}
