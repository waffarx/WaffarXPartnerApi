using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace WaffarXPartnerApi.Application.Common.DTOs.Dashboard;
public class StoreSettingResponseDto
{
    [BsonGuidRepresentation(GuidRepresentation.Standard)]
    public Guid StoreGuid { get; set; }
    public int StoreId { get; set; }
    public string NameAr { get; set; }
    public string NameEn { get; set; }
    public bool IsFeatured { get; set; }
    public int Rank { get; set; }
    public string LogoUrl { get; set; }
    public string LogoPngUrl { get; set; }
}
