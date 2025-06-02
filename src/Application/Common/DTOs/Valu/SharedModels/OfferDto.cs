namespace WaffarXPartnerApi.Application.Common.DTOs.Valu.SharedModels;
public class OfferDto
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public bool IsRewardOffer { get; set; }
    public string TypeName { get; set; }
    public bool IsStoreLevel { get; set; }
    public bool IsProductLevel { get; set; }
}
