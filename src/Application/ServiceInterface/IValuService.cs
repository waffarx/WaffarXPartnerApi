using WaffarXPartnerApi.Application.Common.DTOs.Valu.SharedModels;
using WaffarXPartnerApi.Application.Common.DTOs.Valu.ValuRequestDto.GetFeaturedByStoreRequest;
using WaffarXPartnerApi.Application.Common.DTOs.Valu.ValuRequestDto.GetProductsOffersRequest;
using WaffarXPartnerApi.Application.Common.DTOs.Valu.ValuRequestDto.GetStoreBrands;
using WaffarXPartnerApi.Application.Common.DTOs.Valu.ValuRequestDto.GetStoresRequest;
using WaffarXPartnerApi.Application.Common.DTOs.Valu.ValuRequestDto.GetStoresWithProductsRequest;
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
    Task<GenericResponseWithCount<List<BaseProductSearchResultDto>>> GetFeaturedProducts(GetFeaturedProductByStoreDto product);
    Task<GenericResponse<StoreDetailDto>> GetStoreDetails(Guid StoreId);
    Task<GenericResponseWithCount<List<StoreResponseDto>>> GetStores(GetStoresRequestDto model);
    Task<GenericResponse<string>> CreateExitClick(string section, Guid storeId, string productId = "", string userIdentifier = ""
        ,string shoppingTripIdentifier = "", string variant = "");

    Task<GenericResponse<ProductSearchResultWithFiltersDto>> StoreSearchProduct(StoreProductSearchRequestDto storeProductSearch);
    Task<GenericResponse<StoreCategoriesDto>> GetStoreCategories(Guid storeId);
    Task<GenericResponseWithCount<List<StoreWithOffersResponseDto>>> GetStoresWithOffers(GetStoresRequestDto model);
    Task<GenericResponseWithCount<List<StoreWithProductsResponseDto>>> GetStoresWithProducts(GetStoresWithProductsDto model);
    Task<GenericResponseWithCount<List<BaseProductSearchResultDto>>> GetProductsByOffers(GetProductsByOffersDto offersDto);
    Task<GenericResponseWithCount<List<OfferStoresResponseDto>>> GetStoresByActiveOffers(GetProductsByOffersDto model);
    Task<GenericResponse<List<string>>> SearchBrandsByStore(GetStoreBrandsDto model);
}
