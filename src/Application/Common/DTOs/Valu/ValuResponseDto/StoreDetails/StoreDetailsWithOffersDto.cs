using WaffarXPartnerApi.Application.Common.DTOs.Valu.SharedModels;

namespace WaffarXPartnerApi.Application.Common.DTOs.Valu.ValuResponseDto.StoreDetails;
public class StoreDetailsWithOffersDto : StoreDto
{
    public List<OfferDto> Offers { get; set; }
}
