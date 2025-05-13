using WaffarXPartnerApi.Application.Common.DTOs.Valu.SharedModels;
using WaffarXPartnerApi.Application.Common.DTOs.ValuResponseDto;

namespace WaffarXPartnerApi.Application.Common.DTOs.Valu.ValuResponseDto;
public class StoreWithProductsResponseDto : StoreDto
{
    public string ShoppingUrl { get; set; }
    public string ShoppingUrlBase { get; set; }
    public List<OfferDto> Offers { get; set; }
    public List<BaseProductSearchResultDto> Products { get; set; }
}
