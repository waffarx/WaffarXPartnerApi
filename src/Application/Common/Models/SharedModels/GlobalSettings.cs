﻿using Microsoft.Extensions.Configuration;

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
        public static string SeqServerUrl { get; set; }

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

    public static class ExternalApis
    {
        public static string ValuUrl { get; set; }
        public static string SharedApiUrl { get; set; }
        public static string ExitClickBaseUrl {get; set; }
        public static string EClickAuthBaseUrl {get; set; }

    }
    public static void Initialize(IConfiguration configuration)
    {
        // Connection Strings
        ConnectionStrings.SQlConnection = configuration["ConnectionStrings:SQlConnection"];
        ConnectionStrings.MongoConnection = configuration["ConnectionStrings:MongoConnection"];
        ConnectionStrings.WaffarXConnection = configuration["ConnectionStrings:WaffarXConnection"];
        ConnectionStrings.MongoDatabaseName = configuration["ConnectionStrings:MongoDatabaseName"];

        // Logging
        Logging.LogLevel.Default = configuration["Logging:LogLevel:Default"];
        Logging.LogLevel.Microsoft = configuration["Logging:LogLevel:Microsoft"];
        Logging.LogLevel.MicrosoftHostingLifetime = configuration["Logging:LogLevel:Microsoft.Hosting.Lifetime"];
        Logging.SeqServerUrl = configuration["Logging:SeqServerUrl"];
        // Allowed Hosts
        AllowedHosts = configuration["AllowedHosts"];
        ExternalApis.ValuUrl = configuration["ExternalApis:ValuUrl"];
        ExternalApis.SharedApiUrl = configuration["ExternalApis:SharedApiUrl"];
        ExternalApis.ExitClickBaseUrl = configuration["ExternalApis:ExitClickBaseUrl"];
        ExternalApis.EClickAuthBaseUrl = configuration["ExternalApis:ExitClickAuthBaseUrl"];

        // JWT Settings
        JwtSettings.SecretKey = configuration["JwtSettings:SecretKey"];
        JwtSettings.Issuer = configuration["JwtSettings:Issuer"];
        JwtSettings.Audience = configuration["JwtSettings:Audience"];
        JwtSettings.ExpiryInMinutes = int.Parse(configuration["JwtSettings:ExpiryInMinutes"]);

    }
}
