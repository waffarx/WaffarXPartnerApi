using WaffarXPartnerApi.Application.Common.DTOs.Valu.SharedModels;
using WaffarXPartnerApi.Application.Common.DTOs.Valu.ValuRequestDto.GetFeaturedByStoreRequest;
using WaffarXPartnerApi.Application.Common.DTOs.Valu.ValuRequestDto.GetFeaturedProductRequest;
using WaffarXPartnerApi.Application.Common.DTOs.Valu.ValuRequestDto.GetStoresRequest;
using WaffarXPartnerApi.Application.Common.DTOs.Valu.ValuRequestDto.ProductSearchRequest;
using WaffarXPartnerApi.Application.Common.DTOs.Valu.ValuRequestDto.StoreProductSearchRequest;
using WaffarXPartnerApi.Application.Common.DTOs.Valu.ValuResponseDto;
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
    Task<GenericResponse<string>> CreateExitClick(string section, Guid storeId, string productId = "", string userIdentifier = ""
        ,string shoppingTripIdentifier = "", string variant = "");

    Task<GenericResponse<ProductSearchResultWithFiltersDto>> StoreSearchProduct(StoreProductSearchRequestDto storeProductSearch);
    Task<GenericResponse<StoreCategoriesDto>> GetStoreCategories(Guid storeId);
    Task<GenericResponseWithCount<List<BaseProductSearchResultDto>>> GetFeaturedProductsByStoreId(GetFeaturedProductByStoreDto product);
}
