using WaffarXPartnerApi.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using WaffarXPartnerApi.Domain.RepositoryInterface.EntityFrameworkRepositoryInterface;
using WaffarXPartnerApi.Infrastructure.RepositoryImplementation.EntityFrameworkRepository;
using WaffarXPartnerApi.Application.ServiceInterface;
using WaffarXPartnerApi.Application.ServiceImplementation;
using WaffarXPartnerApi.Application.ServiceInterface.Shared;
using WaffarXPartnerApi.Application.ServiceImplementation.Shared;
using WaffarXPartnerApi.Infrastructure.RepositoryImplementation.EntityFrameworkRepository.WaffarX;
using WaffarXPartnerApi.Domain.RepositoryInterface.EntityFrameworkRepositoryInterface.WaffarX;
using StackExchange.Redis;
using Microsoft.Extensions.Logging;

namespace Microsoft.Extensions.DependencyInjection;
#nullable disable

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("SQlConnection").Replace("{con}", Environment.GetEnvironmentVariable("condb"));
        var redisConnectionString = configuration.GetConnectionString("Redis");
        var waffarxconnectionString = configuration.GetConnectionString("WaffarXConnection");

        Guard.Against.Null(connectionString, message: "Connection string 'SQlConnection' not found.");

        services.AddDbContext<WaffarXPartnerDbContext>((sp, options) =>
        {
            options.UseSqlServer(connectionString);

        });

        services.AddDbContext<WaffarXContext>((sp, options) =>
        {
            options.UseSqlServer(waffarxconnectionString);
            options.UseSqlServer(waffarxconnectionString, o => o.UseCompatibilityLevel(120))
                        .LogTo(Console.WriteLine, LogLevel.Information);
        });
        services.AddMongoDb(configuration);

        services.AddSingleton<IConnectionMultiplexer>(sp =>
        {
            var configuration = redisConnectionString;
            return ConnectionMultiplexer.Connect(configuration);
        });

        services.AddSingleton(TimeProvider.System);
        #region Repositories
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
        services.AddScoped<IApiClientRepository, ApiClientRepository>();
        services.AddScoped<IAdvertiserRepository, AdvertiserRepository>();
        services.AddScoped<IResourceRepository, ResourceRepository>();
        services.AddScoped<ICacheRepository, RedisRepository>();


        services.AddScoped<IResourceService, ResourceService>();
        services.AddScoped<ICacheService, CacheService>();
        #endregion


        //services.AddScoped<IClientService, ClientService>();
        services.AddScoped<IValuService, ValuService>();


        return services;
    }
}
