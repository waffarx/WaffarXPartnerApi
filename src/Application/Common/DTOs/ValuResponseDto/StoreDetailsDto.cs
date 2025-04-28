namespace WaffarXPartnerApi.Application.Common.DTOs.ValuResponseDto;
public class StoreDetailsDto
{
    public StoreDto Store { get; set; }
    public List<ProductSearchResponseModel> StoreFeaturedProducts { get; set; }

}
