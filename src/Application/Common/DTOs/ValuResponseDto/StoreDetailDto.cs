namespace WaffarXPartnerApi.Application.Common.DTOs.ValuResponseDto;
public class StoreDetailDto
{
    public StoreDto Store { get; set; }
    public List<ProductSearchResultDto> StoreFeaturedProducts { get; set; }
}
