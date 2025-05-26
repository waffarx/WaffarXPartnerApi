namespace WaffarXPartnerApi.Domain.Models.PartnerMongoModels;
public class OfferSettingListingModel
{
    public string OfferId { get; set; }
    public string OfferName { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public bool IsProductLevel { get; set; }
    public bool IsStoreLevel { get; set; }
    public string OfferTypeId { get; set; }
}
