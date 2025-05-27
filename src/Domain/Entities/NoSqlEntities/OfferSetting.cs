using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace WaffarXPartnerApi.Domain.Entities.NoSqlEnitities;

[BsonIgnoreExtraElements]
public class OfferSetting
{
    [BsonId]
    public ObjectId Id { get; set; }

    public int CreatedBy { get; set; }

    public int UpdatedBy { get; set; }

    public DateTime CreatedAt { get; set; }

    public int ClientApiId { get; set; }

    public ObjectId OfferLookUpId { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    public bool IsProductLevel { get; set; }

    public bool IsStoreLevel { get; set; }

    public List<ObjectId> ProductIds { get; set; }

    public List<int> StoreIds { get; set; }
    public ObjectId OfferTypeId { get; set; }
    public bool IsFixed { get; set; }
    public int Amount { get; set; }
}
