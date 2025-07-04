﻿namespace WaffarXPartnerApi.Domain.Models.PartnerMongoModels;
public class OfferSettingModel
{
    public string Id { get; set; }
    public string OfferLookUpId { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public bool IsProductLevel { get; set; }
    public bool IsStoreLevel { get; set; }
    public List<int> StoreIds { get; set; }
    public List<int> ProductStoreIds { get; set; }
    public List<string> ProductIds { get; set; }
    public int ClientApiId { get; set; }
    public int UserId { get; set; }
    public string OfferTypeId { get; set; }
    public bool IsFixed { get; set; }
    public int Amount { get; set; }
}
