using WaffarXPartnerApi.Application.Common.DTOs.Dashboard.Products.DeleteFeatured;
using WaffarXPartnerApi.Application.Common.DTOs.Dashboard.Products.FeaturedProducts;
using WaffarXPartnerApi.Application.Common.DTOs.Dashboard.Products.FeaturedProducts.GetFeaturedProduct;
using WaffarXPartnerApi.Application.Common.Models.SharedModels;

namespace WaffarXPartnerApi.Application.ServiceInterface.Dashboard;
public interface IProductSettingService
{
    Task<GenericResponseWithCount<List<FeaturedProductResponseDto>>> GetFeaturedProducts(GetPartnerFeaturedProductDto featuredProductDto);
    Task<GenericResponse<bool>> DeleteFeaturedProduct(DeleteFeaturedProductDto productDto);
}
