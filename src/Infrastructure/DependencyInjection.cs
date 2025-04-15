using WaffarXPartnerApi.Application.Common.Interfaces;
using WaffarXPartnerApi.Domain.Constants;
using WaffarXPartnerApi.Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;

namespace Microsoft.Extensions.DependencyInjection;
#nullable disable

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("SQlConnection").Replace("{con}", Environment.GetEnvironmentVariable("condb"));

        Guard.Against.Null(connectionString, message: "Connection string 'SQlConnection' not found.");

        //services.AddScoped<ISaveChangesInterceptor, AuditableEntityInterceptor>();
        //services.AddScoped<ISaveChangesInterceptor, DispatchDomainEventsInterceptor>();

        services.AddDbContext<WaffarXPartnerDbContext>((sp, options) =>
        {
            options.AddInterceptors(sp.GetServices<ISaveChangesInterceptor>());

            options.UseSqlServer(connectionString);
        });
        services.AddMongoDb(configuration);

       // services.AddScoped<ApplicationDbContextInitialiser>();

        services.AddAuthentication()
            .AddBearerToken(IdentityConstants.BearerScheme);

        services.AddAuthorizationBuilder();

        //services
        //    //.AddIdentityCore<ApplicationUser>()
        //    //.AddRoles<IdentityRole>()
        //    .AddEntityFrameworkStores<WaffarXPartnerDbContext>()
        //    .AddApiEndpoints();

        services.AddSingleton(TimeProvider.System);

        services.AddAuthorization(options =>
            options.AddPolicy(Policies.CanPurge, policy => policy.RequireRole(Roles.Administrator)));

        return services;
    }
}
