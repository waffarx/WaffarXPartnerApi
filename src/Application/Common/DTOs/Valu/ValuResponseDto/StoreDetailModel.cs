using WaffarXPartnerApi.Application.Common.DTOs.Valu.SharedModels;

namespace WaffarXPartnerApi.Application.Common.DTOs.ValuResponseDto;
public class StoreDetailModel
{
    public StoreDto Store { get; set; }
    public List<ProductSearchResponseModel> StoreFeaturedProducts { get; set; }

}
