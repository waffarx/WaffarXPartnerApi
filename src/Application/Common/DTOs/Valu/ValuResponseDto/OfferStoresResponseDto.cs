using WaffarXPartnerApi.Application.Common.DTOs.Valu.SharedModels;

namespace WaffarXPartnerApi.Application.Common.DTOs.Valu.ValuResponseDto;
public class OfferStoresResponseDto : OfferDto
{
    public string Description { get; set; }
    public List<StoreResponseDto> Stores { get; set; }

}
