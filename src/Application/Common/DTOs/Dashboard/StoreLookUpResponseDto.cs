using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace WaffarXPartnerApi.Application.Common.DTOs.Dashboard;
public class StoreLookUpResponseDto
{
    [BsonGuidRepresentation(GuidRepresentation.Standard)]
    public Guid StoreGuid { get; set; }
    public int StoreId { get; set; }
    public string NameAr { get; set; }
    public string NameEn { get; set; }
    public string LogoUrl { get; set; }
    public string LogoPngUrl { get; set; }
}
