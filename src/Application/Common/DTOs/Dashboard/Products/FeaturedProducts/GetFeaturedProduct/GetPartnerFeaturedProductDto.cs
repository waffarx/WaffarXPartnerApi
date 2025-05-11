namespace WaffarXPartnerApi.Application.Common.DTOs.Dashboard.Products.FeaturedProducts.GetFeaturedProduct;
public class GetPartnerFeaturedProductDto
{
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public bool IsActive { get; set; } = false;
}
