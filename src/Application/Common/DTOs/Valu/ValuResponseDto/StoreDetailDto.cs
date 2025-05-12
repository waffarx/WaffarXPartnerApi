using WaffarXPartnerApi.Application.Common.DTOs.Valu.SharedModels;

namespace WaffarXPartnerApi.Application.Common.DTOs.ValuResponseDto;
public class StoreDetailDto
{
    public StoreResponseDto Store { get; set; }
    public List<BaseProductSearchResultDto> StoreFeaturedProducts { get; set; }
    public List<OfferDto> Offers { get; set; }
}
