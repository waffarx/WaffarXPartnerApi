namespace WaffarXPartnerApi.Application.Common.DTOs.Dashboard.Offers;
public class OfferSettingRequestDto
{
    public Guid? Id { get; set; }
    public Guid OfferLookUpId { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public bool IsProductLevel { get; set; }
    public bool IsStoreLevel { get; set; }
    public List<int> StoreIds { get; set; }
    public List<Guid> ProductIds { get; set; }
}
