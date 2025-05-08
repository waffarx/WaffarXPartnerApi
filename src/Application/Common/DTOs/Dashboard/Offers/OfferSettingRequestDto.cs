namespace WaffarXPartnerApi.Application.Common.DTOs.Dashboard.Offers;

public class OfferSettingRequestDto
{
    public string Id { get; set; }
    public string OfferLookUpId { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public bool IsProductLevel { get; set; }
    public bool IsStoreLevel { get; set; }
    public List<string> StoreIds { get; set; }
    public List<string> ProductIds { get; set; }
}
