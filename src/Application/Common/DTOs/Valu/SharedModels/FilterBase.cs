namespace WaffarXPartnerApi.Application.Common.DTOs.Valu.SharedModels;
public class FilterBase
{
    public List<string> Brands { get; set; }
    public double MinPrice { get; set; }
    public double MaxPrice { get; set; }
}
