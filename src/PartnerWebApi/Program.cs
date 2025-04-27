using PartnerWebApi.Infrastructure;
using WaffarXPartnerApi.Application.Common.Models.SharedModels;
using WaffarXPartnerApi.Application.ServiceImplementation;
using WaffarXPartnerApi.Application.ServiceImplementation.Shared;
using WaffarXPartnerApi.Application.ServiceInterface;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddKeyVaultIfConfigured(builder.Configuration);

builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddWebServices();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddControllers();

builder.Services.AddScoped<IClientService, ClientService>();
builder.Services.AddHttpClient<IHttpService, HttpService>(client =>
{
    client.BaseAddress = new Uri(AppSettings.ExternalApis.SharedApiUrl);
    client.DefaultRequestHeaders.Add("Accept", "application/json");
});
builder.Services.AddHttpContextAccessor();

var app = builder.Build();
AppSettings.Initialize(builder.Configuration);

app.MapGet("/web", () => Results.Redirect("/swagger"));
app.UseOpenApi();
app.UseSwaggerUi(settings =>
{
    settings.Path = "/swagger";

    settings.DocumentPath = "/swagger/v1/swagger.json";
});
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    //await app.InitialiseDatabaseAsync();
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

app.Run();

public partial class Program { }
