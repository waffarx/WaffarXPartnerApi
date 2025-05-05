namespace WaffarXPartnerApi.Application.Common.DTOs.Dashboard.Offers;
public class OfferResponseDto
{
    public Guid OfferId { get; set; }
    public string OfferName { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public bool IsProductLevel { get; set; }
    public bool IsStoreLevel { get; set; }
}
