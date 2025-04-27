using Microsoft.Extensions.Configuration;

namespace WaffarXPartnerApi.Application.Common.Models.SharedModels;
#nullable disable
/// <summary>
/// Read global settings from appsettings.json
/// </summary>

public static class AppSettings
{
    public static class ConnectionStrings
    {
        public static string MongoConnection { get;  set; }
        public static string SQlConnection { get;  set; }
        public static string MongoDatabaseName { get;  set; }
        public static string WaffarXConnection { get;  set; }
    }

    public static class Logging
    {
        public static class LogLevel
        {
            public static string Default { get;  set; }
            public static string Microsoft { get;  set; }
            public static string MicrosoftHostingLifetime { get;  set; }
        }
    }

    public static string AllowedHosts { get;  set; }

    public static class JwtSettings
    {
        public static string SecretKey { get;  set; }
        public static string Issuer { get;  set; }
        public static string Audience { get;  set; }
        public static int ExpiryInMinutes { get;  set; }
    }

    public static class Serilog
    {
        public static string[] Using { get;  set; }

        public static class MinimumLevel
        {
            public static string Default { get;  set; }
            public static string OverrideMicrosoft { get;  set; }
            public static string OverrideSystem { get;  set; }
        }

        public static class WriteTo
        {
            public static string FilePath { get;  set; }
            public static string FileRollingInterval { get;  set; }
        }

        public static string[] Enrich { get;  set; }
        public static string Application { get;  set; }
    }

    public static class WaffarXSettings
    {
        public static string BaseImgUrl { get;  set; }
        public static string ExitClickBaseUrl { get;  set; }
        public static string MobExitClickBaseUrl { get;  set; }
        public static string ECBaseUrl { get;  set; }
        public static string InStoreBackgroundImage { get;  set; }
        public static string ErrorPage { get;  set; }
        public static string Environment { get;  set; }
        public static string FlagLink { get;  set; }
        public static string ProductsError { get;  set; }
        public static string ZenDeskUrl { get;  set; }
        public static string ZenDeskToken { get; set; }
        public static List<string> StorePageSections { get;  set; }
        public static string RecaptchaPrivateKey { get; set; }
        public static string RecaptchaBaseUrl { get; set; }
    }
    public static class ExternalApis
    {
        public static string ValuUrl { get; set; }
        public static string SharedApiUrl { get; set; }
    }
    public static void Initialize(IConfiguration configuration)
    {
        // Connection Strings
        ConnectionStrings.SQlConnection = configuration["ConnectionStrings:SQlConnection"];
        ConnectionStrings.MongoConnection = configuration["ConnectionStrings:MongoConnection"];
        ConnectionStrings.WaffarXConnection = configuration["ConnectionStrings:WaffarXConnection"];
        ConnectionStrings.MongoDatabaseName = configuration["ConnectionStrings:MongoDatabaseName"];
        //ConnectionStrings.Redis = configuration["ConnectionStrings:Redis"];

        // Logging
        Logging.LogLevel.Default = configuration["Logging:LogLevel:Default"];
        Logging.LogLevel.Microsoft = configuration["Logging:LogLevel:Microsoft"];
        Logging.LogLevel.MicrosoftHostingLifetime = configuration["Logging:LogLevel:Microsoft.Hosting.Lifetime"];

        // Allowed Hosts
        AllowedHosts = configuration["AllowedHosts"];
        ExternalApis.ValuUrl = configuration["ExternalApis:ValuUrl"];
        ExternalApis.SharedApiUrl = configuration["ExternalApis:SharedApiUrl"];

        //// JWT Settings
        //JwtSettings.SecretKey = configuration["JwtSettings:SecretKey"];
        //JwtSettings.Issuer = configuration["JwtSettings:Issuer"];
        //JwtSettings.Audience = configuration["JwtSettings:Audience"];
        //JwtSettings.ExpiryInMinutes = int.Parse(configuration["JwtSettings:ExpiryInMinutes"]);

        //// Serilog Settings
        //Serilog.Using = configuration.GetSection("Serilog:Using").ToString().Split(',');
        //Serilog.MinimumLevel.Default = configuration["Serilog:MinimumLevel:Default"];
        //Serilog.MinimumLevel.OverrideMicrosoft = configuration["Serilog:MinimumLevel:Override:Microsoft"];
        //Serilog.MinimumLevel.OverrideSystem = configuration["Serilog:MinimumLevel:Override:System"];
        //Serilog.WriteTo.FilePath = configuration["Serilog:WriteTo:1:Args:path"];
        //Serilog.WriteTo.FileRollingInterval = configuration["Serilog:WriteTo:1:Args:rollingInterval"];
        //Serilog.Enrich = configuration.GetSection("Serilog:Enrich").ToString().Split(',');
        //Serilog.Application = configuration["Serilog:Properties:Application"];

        //// WaffarX Settings
        //WaffarXSettings.BaseImgUrl = configuration["WaffarXSettings:BaseImgUrl"];
        //WaffarXSettings.ExitClickBaseUrl = configuration["WaffarXSettings:ExitClickBaseUrl"];
        //WaffarXSettings.MobExitClickBaseUrl = configuration["WaffarXSettings:MobExitClickBaseUrl"];
        //WaffarXSettings.ECBaseUrl = configuration["WaffarXSettings:ECBaseUrl"];
        //WaffarXSettings.InStoreBackgroundImage = configuration["WaffarXSettings:InStoreBackgroundImage"];
        //WaffarXSettings.ErrorPage = configuration["WaffarXSettings:ErrorPage"];
        //WaffarXSettings.Environment = configuration["WaffarXSettings:Environment"];
        //WaffarXSettings.FlagLink = configuration["WaffarXSettings:FlagLink"];
        //WaffarXSettings.ProductsError = configuration["WaffarXSettings:ProductsError"];
        //WaffarXSettings.ZenDeskUrl = configuration["WaffarXSettings:ZenDeskUrl"];
        //WaffarXSettings.ZenDeskToken = configuration["WaffarXSettings:ZenDeskToken"];
        //WaffarXSettings.StorePageSections = configuration.GetSection("WaffarXSettings:StorePageSections").GetChildren().Select(s => s.Value).ToList();
        //WaffarXSettings.RecaptchaPrivateKey = configuration["WaffarXSettings:RecaptchaPrivateKey"];
        //WaffarXSettings.RecaptchaBaseUrl = configuration["WaffarXSettings:RecaptchaBaseUrl"];


    }
}
