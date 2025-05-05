using System.Reflection;
using WaffarXPartnerApi.Application.Common.DTOs.Valu.SharedModels;
using WaffarXPartnerApi.Application.Common.DTOs.Valu.ValuRequestDto.GetStoresRequest;
using WaffarXPartnerApi.Application.Common.DTOs.Valu.ValuRequestDto.PaginationRequest;
using WaffarXPartnerApi.Application.Common.DTOs.Valu.ValuRequestDto.ProductSearchRequest;
using WaffarXPartnerApi.Application.Common.DTOs.Valu.ValuRequestDto.StoreProductSearchRequest;

namespace Microsoft.Extensions.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddAutoMapper(Assembly.GetExecutingAssembly());

        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

        services.AddScoped<IValidator<ProductSearchRequestDto>, ProductSearchRequestDtoValidator>();
        services.AddScoped<IValidator<GetStoresRequestDto>, GetStoresRequestDtoValidator>();
        services.AddScoped<IValidator<StoreProductSearchRequestDto>, StoreProductSearchRequestDtoValidator>();

        return services;
    }
}
