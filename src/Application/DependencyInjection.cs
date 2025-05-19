using System.Reflection;
using WaffarXPartnerApi.Application.Common.DTOs.Dashboard.Postback;
using WaffarXPartnerApi.Application.Common.DTOs.Helper;
using WaffarXPartnerApi.Application.Common.DTOs.Valu.ValuRequestDto.GetFeaturedByStoreRequest;
using WaffarXPartnerApi.Application.Common.DTOs.Valu.ValuRequestDto.GetProductsOffersRequest;
using WaffarXPartnerApi.Application.Common.DTOs.Valu.ValuRequestDto.GetStoresRequest;
using WaffarXPartnerApi.Application.Common.DTOs.Valu.ValuRequestDto.GetStoresWithProductsRequest;
using WaffarXPartnerApi.Application.Common.DTOs.Valu.ValuRequestDto.PaginationRequest;
using WaffarXPartnerApi.Application.Common.DTOs.Valu.ValuRequestDto.ProductSearchRequest;
using WaffarXPartnerApi.Application.Common.DTOs.Valu.ValuRequestDto.StoreProductSearchRequest;
using WaffarXPartnerApi.Application.Common.Mappings;

namespace Microsoft.Extensions.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(MappingProfile));
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

        services.AddScoped<IValidator<ProductSearchRequestDto>, ProductSearchRequestDtoValidator>();
        services.AddScoped<IValidator<GetStoresRequestDto>, GetStoresRequestDtoValidator>();
        services.AddScoped<IValidator<StoreProductSearchRequestDto>, StoreProductSearchRequestDtoValidator>();
        services.AddScoped<IValidator<GetFeaturedProductByStoreDto>, GetFeaturedProductByStoreDtoValidator>();
        services.AddScoped<IValidator<GetStoresWithProductsDto>, GetStoresWithProductsDtoValidator>();
        services.AddScoped<IValidator<PostbackDto>, PostbackDtoValidator>();
        services.AddScoped<IValidator<GetProductsByOffersDto>, GetProductsByOffersDtoValidators>();
        services.AddScoped<RandomGenerator>();
        return services;
    }
}
