using WaffarXPartnerApi.Application.Common.DTOs.ValuRequestDto;
using WaffarXPartnerApi.Application.Common.DTOs.ValuResponseDto;
namespace WaffarXPartnerApi.Application.ServiceInterface;
public interface IValuService
{
    Task<List<ProductSearchResultDto>> SearchProduct(ProductSearchRequestDto productSearch);
    Task<ProductSearchResultDto> GetProductDetails(ProductById product);
    Task<Guid> GetFeaturedProducts(GetFeaturedProductDto product);
    Task<Guid> GetStoreDetails(GetStoreDto store);
    Task<Guid> GetStores();
}
