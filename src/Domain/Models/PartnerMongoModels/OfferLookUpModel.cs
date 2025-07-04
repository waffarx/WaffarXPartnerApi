﻿namespace WaffarXPartnerApi.Domain.Models.PartnerMongoModels;
public class OfferLookUpModel
{
    public string Id { get; set; }
    public string NameAr { get; set; }
    public string NameEn { get; set; }
    public string DescriptionAr { get; set; }
    public string DescriptionEn { get; set; }
    public int ClientApiId { get; set; }
    public int UserId { get; set; }
}
