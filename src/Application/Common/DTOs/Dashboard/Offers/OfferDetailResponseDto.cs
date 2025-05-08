using WaffarXPartnerApi.Application.Common.DTOs.Valu.SharedModels;
using WaffarXPartnerApi.Application.Common.DTOs.ValuResponseDto;

namespace WaffarXPartnerApi.Application.Common.DTOs.Dashboard.Offers;
public class OfferDetailResponseDto
{
    public bool IsProductLevel { get; set; }
    public bool IsStoreLevel { get; set; }
    public string OfferLookupId { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public List<StoreDto> Stores { get; set; }
    public List<BaseProductSearchResultDto> Products { get; set; }


}
