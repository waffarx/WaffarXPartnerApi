using WaffarXPartnerApi.Application.Common.DTOs.Valu.SharedModels;
using WaffarXPartnerApi.Application.Common.DTOs.Valu.ValuResponseDto.StoreDetails;

namespace WaffarXPartnerApi.Application.Common.DTOs.ValuResponseDto;
public class StoreDetailModel
{
    public StoreDetailsWithOffersDto Store { get; set; }
    public List<ProductSearchResponseModel> StoreFeaturedProducts { get; set; }

}
