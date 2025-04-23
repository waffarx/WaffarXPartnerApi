using WaffarXPartnerApi.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using WaffarXPartnerApi.Domain.RepositoryInterface.EntityFrameworkRepositoryInterface;
using WaffarXPartnerApi.Infrastructure.RepositoryImplementation.EntityFrameworkRepository;
using WaffarXPartnerApi.Application.ServiceInterface;
using WaffarXPartnerApi.Application.ServiceImplementation;

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

        Guard.Against.Null(waffarxconnectionString, message: "Connection string 'WaffarXConnection' not found.");
        services.AddDbContext<WaffarXContext>((sp, options) =>
        {
            options.AddInterceptors(sp.GetServices<ISaveChangesInterceptor>());

            options.UseSqlServer(waffarxconnectionString);
        });
        services.AddMongoDb(configuration);

        services.AddSingleton(TimeProvider.System);
        #region Repositories
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
        services.AddScoped<IApiClientRepository, ApiClientRepository>();
        #endregion


        //services.AddScoped<IClientService, ClientService>();
        services.AddScoped<IValuService, ValuService>();


        return services;
    }
}
