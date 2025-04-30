using WaffarXPartnerApi.Application.Common.DTOs.Valu.SharedModels;
using WaffarXPartnerApi.Application.Common.DTOs.ValuRequestDto;
using WaffarXPartnerApi.Application.Common.DTOs.ValuResponseDto;
using WaffarXPartnerApi.Application.Common.Models.SharedModels;
namespace WaffarXPartnerApi.Application.ServiceInterface;
public interface IValuService
{
    Task<GenericResponse<ProductSearchResultWithFiltersDto>> SearchProduct(ProductSearchRequestDto productSearch);
    Task<GenericResponse<DetailedProductSearchResultDto>> GetProductDetails(string id);
    Task<GenericResponseWithCount<List<BaseProductSearchResultDto>>> GetFeaturedProducts(GetFeaturedProductDto product);
    Task<GenericResponse<StoreDetailDto>> GetStoreDetails(Guid StoreId);
    Task<GenericResponseWithCount<List<StoreResponseDto>>> GetStores(GetStoresRequestDto model);
    Task<GenericResponse<int>> CreateExitClick(string section, Guid storeId, string productId = "");
    Task<string> CreateExitClick(string section, Guid storeId, string productId = "", string userIdentifier = ""
        ,string shoppingTripIdentifier = "", string variant = "");
}
