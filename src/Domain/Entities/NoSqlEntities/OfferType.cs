using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace WaffarXPartnerApi.Domain.Entities.NoSqlEntities;

[BsonIgnoreExtraElements]
public class OfferType
{
    [BsonId]
    public ObjectId Id { get; set; }
    public string NameEn { get; set; }
    public string NameAr { get; set; }
    public int ClientApiId { get; set; }
    public int CreatedBy { get; set; }
    public int UpdatedBy { get; set; }
    public DateTime CreatedAt { get; set; }
    public bool IsReward { get; set; }
}
