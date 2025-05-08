using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace WaffarXPartnerApi.Domain.Models.PartnerMongoModels;
public class StoreIdsModel
{
    public int StoreId { get; set; }

    [BsonGuidRepresentation(GuidRepresentation.Standard)]
    public Guid StoreGuid { get; set; }
}
