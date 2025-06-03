using WaffarXPartnerApi.Application.Common.DTOs.Valu.SharedModels;

namespace WaffarXPartnerApi.Application.Common.DTOs.Valu.ValuRequestDto;
public class ProductSearchFilterDto : FilterBase
{
    public string OfferId { get; set; }
    public List<string> Stores { get; set; }
    public bool Discounted { get; set; }

}
