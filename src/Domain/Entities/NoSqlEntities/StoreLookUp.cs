using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace WaffarXPartnerApi.Domain.Entities.NoSqlEnitities;
public class StoreLookUp
{
    [BsonId]
    public ObjectId Id { get; set; }
    public int StoreId { get; set; }
    public Guid StoreGuid { get; set; }
    public string NameAr { get; set; }
    public string NameEn { get; set; }
    public string LogoUrl { get; set; }
    public string LogoPngUrl { get; set; }
    public bool IsActive { get; set; }
}
