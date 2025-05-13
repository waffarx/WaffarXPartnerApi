using WaffarXPartnerApi.Application.Common.DTOs.Valu.SharedModels;

namespace WaffarXPartnerApi.Application.Common.DTOs.ValuResponseDto;
public class StoreDetailDto
{
    public StoreWithOffersResponseDto Store { get; set; }
    public List<BaseProductSearchResultDto> StoreFeaturedProducts { get; set; }
}
