using WaffarXPartnerApi.Application.Common.DTOs.Valu.SharedModels;
using WaffarXPartnerApi.Application.Common.DTOs.ValuResponseDto;

namespace WaffarXPartnerApi.Application.Common.DTOs.Dashboard.Offers;
public class OfferDetailResponseDto
{
    public List<StoreDto> Stores { get; set; }
    public List<BaseProductSearchResultDto> Products { get; set; }

}
