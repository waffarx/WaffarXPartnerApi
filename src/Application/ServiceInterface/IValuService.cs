using WaffarXPartnerApi.Application.Common.DTOs.ValuRequestDto;
using WaffarXPartnerApi.Application.Common.DTOs.ValuResponseDto;
using WaffarXPartnerApi.Application.Common.Models.SharedModels;
namespace WaffarXPartnerApi.Application.ServiceInterface;
public interface IValuService
{
    Task<GenericResponse<ProductSearchResultWithFiltersDto>> SearchProduct(ProductSearchRequestDto productSearch);
    Task<GenericResponse<ProductSearchResultDto>> GetProductDetails(string id);
    Task<GenericResponse<List<ProductSearchResultDto>>> GetFeaturedProducts(GetFeaturedProductDto product);
    Task<GenericResponse<StoreDetailDto>> GetStoreDetails(Guid StoreId);
    Task<GenericResponse<List<StoreDto>>> GetStores(GetStoresRequestDto model);
    Task<GenericResponse<int>> CreateExitClick(string section, Guid storeId, string productId = "");
}
