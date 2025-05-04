using FluentValidation.AspNetCore;
using FluentValidation;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using PartnerWebApi.Infrastructure;
using Serilog;
using Serilog.Exceptions;
using WaffarXPartnerApi.Application.Common.Models.SharedModels;
using WaffarXPartnerApi.Application.ServiceImplementation;
using WaffarXPartnerApi.Application.ServiceImplementation.Shared;
using WaffarXPartnerApi.Application.ServiceInterface;

var builder = WebApplication.CreateBuilder(args);
AppSettings.Initialize(builder.Configuration);


builder.Services.AddControllers(options =>
{
    options.Filters.Add<ValidationFilter>();
});
builder.Services.AddFluentValidationAutoValidation();

// Add services to the container.
builder.Services.AddKeyVaultIfConfigured(builder.Configuration);
builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddWebServices();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddScoped<IClientService, ClientService>();

builder.Services.AddHttpClient<IHttpService, HttpService>(client =>
{
    client.BaseAddress = new Uri(AppSettings.ExternalApis.SharedApiUrl);
    client.DefaultRequestHeaders.Add("Accept", "application/json");
});
builder.Services.AddHttpContextAccessor();
builder.Services.Configure<KestrelServerOptions>(options =>
{
    options.AllowSynchronousIO = true;
});
// Configure Serilog with Seq for exception-only logging
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Error() // Only log errors and fatal events
    .Enrich.FromLogContext()
    .Enrich.WithMachineName()
    .Enrich.WithExceptionDetails() // Adds detailed exception information
    .WriteTo.Seq(AppSettings.Logging.SeqServerUrl)
    .CreateLogger();

builder.Host.UseSerilog();

var app = builder.Build();

app.MapGet("/web", () => Results.Redirect("/swagger"));
app.UseOpenApi();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwaggerUi(settings =>
    {
        settings.Path = "/swagger";

        settings.DocumentPath = "/swagger/v1/swagger.json";
    });
}
else
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHealthChecks("/health");
app.UseHttpsRedirection();
app.UseStaticFiles();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action=Index}/{id?}");

app.MapRazorPages();

app.MapFallbackToFile("index.html");

app.UseExceptionHandler(options => { });

app.MapEndpoints();
app.UseMiddleware<AuthenticationMiddleware>();
app.UseExceptionHandlingMiddleware();

app.Run();

public partial class Program { }
