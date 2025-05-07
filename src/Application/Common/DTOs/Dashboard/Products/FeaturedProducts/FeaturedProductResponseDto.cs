using WaffarXPartnerApi.Application.Common.DTOs.ValuResponseDto;

namespace WaffarXPartnerApi.Application.Common.DTOs.Dashboard.Products.FeaturedProducts;
public class FeaturedProductResponseDto
{
    public string ProductId { get; set; }
    public int Rank { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public BaseProductSearchResultDto ProductDetails { get; set; }
}
