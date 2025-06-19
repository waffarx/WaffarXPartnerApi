using MongoDB.Bson.Serialization.Attributes;

namespace WaffarXPartnerApi.Domain.Entities.NoSqlEntities;

[BsonIgnoreExtraElements]
public class StoreSettings
{
    public int StoreId { get; set; }
    public bool IsFeatured { get; set; }
    public int Rank { get; set; }
    public string BackgroundColor { get; set; }

}
