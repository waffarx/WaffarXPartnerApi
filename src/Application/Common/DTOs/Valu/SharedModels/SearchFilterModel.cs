namespace WaffarXPartnerApi.Application.Common.DTOs.Valu.SharedModels;
public class SearchFilterModel : FilterBase
{
    public string OfferId { get; set; }
    public string Brands { get; set; }
    public List<int> Stores { get; set; }

}
