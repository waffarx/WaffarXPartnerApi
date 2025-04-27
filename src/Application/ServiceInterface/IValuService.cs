using WaffarXPartnerApi.Application.Common.DTOs.ValuRequestDto;
namespace WaffarXPartnerApi.Application.ServiceInterface;
public interface IValuService
{
    Task<Guid> SearchProduct(ProductSearchDto productSearch);
    Task<Guid> GetProductDetails(ProductById product);
    Task<Guid> GetFeaturedProducts(GetFeaturedProductDto product);
    Task<Guid> GetStoreDetails(GetStoreDto store);
}
