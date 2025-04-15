using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using WaffarXPartnerApi.Domain.Entities.NoSqlEnitities;

namespace WaffarXPartnerApi.Infrastructure.Data
{
    public static class MongoDbConfiguration
    {
        public static IServiceCollection AddMongoDb(this IServiceCollection services, IConfiguration configuration)
        {
            // Register MongoDB client
            services.AddSingleton<IMongoClient>(sp =>
            {
                var connectionString = configuration.GetConnectionString("MongoDb");
                return new MongoClient(connectionString);
            });

            // Register MongoDB database
            services.AddSingleton(sp =>
            {
                var client = sp.GetRequiredService<IMongoClient>();
                var databaseName = configuration["ConnectionStrings:MongoDatabaseName"];
                return client.GetDatabase(databaseName);
            });

            // Register collections
            services.AddSingleton<IMongoCollection<FeaturedProductSetting>>(sp =>
            {
                var database = sp.GetRequiredService<IMongoDatabase>();
                return database.GetCollection<FeaturedProductSetting>("FeaturedProductSetting");
            });

            services.AddSingleton<IMongoCollection<FeaturedProductSettingAudit>>(sp =>
            {
                var database = sp.GetRequiredService<IMongoDatabase>();
                return database.GetCollection<FeaturedProductSettingAudit>("FeaturedProductSettingAudit");
            });

            services.AddSingleton<IMongoCollection<ProductSearchSetting>>(sp =>
            {
                var database = sp.GetRequiredService<IMongoDatabase>();
                return database.GetCollection<ProductSearchSetting>("ProductSearchSetting");
            });

            services.AddSingleton<IMongoCollection<ProductSearchSettingAudit>>(sp =>
            {
                var database = sp.GetRequiredService<IMongoDatabase>();
                return database.GetCollection<ProductSearchSettingAudit>("ProductSearchSettingAudit");
            });

            services.AddSingleton<IMongoCollection<OfferLookUp>>(sp =>
            {
                var database = sp.GetRequiredService<IMongoDatabase>();
                return database.GetCollection<OfferLookUp>("OfferLookUp");
            });

            services.AddSingleton<IMongoCollection<OfferSetting>>(sp =>
            {
                var database = sp.GetRequiredService<IMongoDatabase>();
                return database.GetCollection<OfferSetting>("OfferSetting");
            });

            services.AddSingleton<IMongoCollection<OfferSettingAudit>>(sp =>
            {
                var database = sp.GetRequiredService<IMongoDatabase>();
                return database.GetCollection<OfferSettingAudit>("OfferSettingAudit");
            });

            services.AddSingleton<IMongoCollection<StoreSearchSetting>>(sp =>
            {
                var database = sp.GetRequiredService<IMongoDatabase>();
                return database.GetCollection<StoreSearchSetting>("StoreSearchSetting");
            });

            services.AddSingleton<IMongoCollection<StoreSearchSettingAudit>>(sp =>
            {
                var database = sp.GetRequiredService<IMongoDatabase>();
                return database.GetCollection<StoreSearchSettingAudit>("StoreSearchSettingAudit");
            });
            services.AddSingleton<IMongoCollection<StoreLookUp>>(sp =>
            {
                var database = sp.GetRequiredService<IMongoDatabase>();
                return database.GetCollection<StoreLookUp>("StoreLookUp");
            });
            return services;
        }
    }
}
