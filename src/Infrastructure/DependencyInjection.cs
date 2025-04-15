using WaffarXPartnerApi.Infrastructure.Data;
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


        services.AddDbContext<WaffarXPartnerDbContext>((sp, options) =>
        {
            options.AddInterceptors(sp.GetServices<ISaveChangesInterceptor>());

            options.UseSqlServer(connectionString);
        });
        var waffarxconnectionString = configuration.GetConnectionString("WaffarXConnection").Replace("{con}", Environment.GetEnvironmentVariable("condb"));

        Guard.Against.Null(connectionString, message: "Connection string 'WaffarXConnection' not found.");
        services.AddDbContext<WaffarXContext>((sp, options) =>
        {
            options.AddInterceptors(sp.GetServices<ISaveChangesInterceptor>());

            options.UseSqlServer(connectionString);
        });
        services.AddMongoDb(configuration);

        services.AddSingleton(TimeProvider.System);

        //services.AddScoped<ISaveChangesInterceptor, AuditableEntityInterceptor>();
        //services.AddScoped<ISaveChangesInterceptor, DispatchDomainEventsInterceptor>();
        // services.AddScoped<ApplicationDbContextInitialiser>();

        //services.AddAuthentication()
        //    .AddBearerToken(IdentityConstants.BearerScheme);

        //services.AddAuthorizationBuilder();

        //services
        //    //.AddIdentityCore<ApplicationUser>()
        //    //.AddRoles<IdentityRole>()
        //    .AddEntityFrameworkStores<WaffarXPartnerDbContext>()
        //    .AddApiEndpoints();


        //services.AddAuthorization(options =>
        //    options.AddPolicy(Policies.CanPurge, policy => policy.RequireRole(Roles.Administrator)));

        return services;
    }
}
