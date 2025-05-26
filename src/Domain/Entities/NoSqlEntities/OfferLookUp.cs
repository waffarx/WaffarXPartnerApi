using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace WaffarXPartnerApi.Domain.Entities.NoSqlEnitities;
public class OfferLookUp
{
    [BsonId]
    public ObjectId Id { get; set; }

    public int CreatedBy { get; set; }

    public int UpdatedBy { get; set; }

    public DateTime CreatedAt { get; set; }

    public int ClientApiId { get; set; }

    public string UniqueKey { get; set; }

    public string NameAr { get; set; }

    public string NameEn { get; set; }
    public string DescriptionAr { get; set; }
    public string DescriptionEn { get; set; }
}
