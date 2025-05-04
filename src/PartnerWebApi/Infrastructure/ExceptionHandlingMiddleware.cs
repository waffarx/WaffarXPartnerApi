using Serilog.Context;
using System.Text;
using System.Text.Json;
using Microsoft.IO;
using WaffarXPartnerApi.Application.Common.Models.SharedModels;
using WaffarXPartnerApi.Domain.Models.SharedModels;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;
    private readonly RecyclableMemoryStreamManager _recyclableMemoryStreamManager;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
        _recyclableMemoryStreamManager = new RecyclableMemoryStreamManager();
    }

    public async Task InvokeAsync(HttpContext context)
    {
        // Store the original body stream
        var originalBodyStream = context.Response.Body;

        // Capture the request body
        string requestBody = await GetRequestBodyAsync(context.Request);

        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            // Use Serilog's LogContext to add properties that will appear in Seq
            using (LogContext.PushProperty("RequestPath", context.Request.Path))
            using (LogContext.PushProperty("ClientIP", context.Connection.RemoteIpAddress?.ToString()))
            using (LogContext.PushProperty("UserAgent", context.Request.Headers["User-Agent"].ToString()))
            using (LogContext.PushProperty("RequestBody", requestBody)) // Add the request body to logs
            {
                // This error will be sent to Seq
                _logger.LogError(ex, "Unhandled exception in {EndpointPath}", context.Request.Path);
            }
            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task<string> GetRequestBodyAsync(HttpRequest request)
    {
        // Ensure the request body can be read multiple times
        request.EnableBuffering();

        // Read the request body
        using var streamReader = new StreamReader(
            request.Body,
            encoding: Encoding.UTF8,
            detectEncodingFromByteOrderMarks: false,
            leaveOpen: true);

        var requestBody = await streamReader.ReadToEndAsync();

        // Reset the position to the beginning for the next reader
        request.Body.Position = 0;

        return requestBody;
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        // Your existing code...
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = exception switch
        {
            ArgumentException => StatusCodes.Status400BadRequest,
            UnauthorizedAccessException => StatusCodes.Status401Unauthorized,
            KeyNotFoundException => StatusCodes.Status404NotFound,
            _ => StatusCodes.Status404NotFound
        };

        var environment = context.RequestServices.GetService<IWebHostEnvironment>();
        var isDevelopment = environment?.IsDevelopment() ?? false;

        var errorResponse = new GenericResponse<object>() { Status = StaticValues.Error, Errors = new List<string>() };

        //var errorResponse = new
        //{
        //    message = exception.Message,
        //    statusCode = context.Response.StatusCode,
        //    detailedError = isDevelopment ? exception.StackTrace : null
        //};

        return context.Response.WriteAsync(JsonSerializer.Serialize(errorResponse));
    }
}

// Extension method
public static class ExceptionHandlingMiddlewareExtensions
{
    public static IApplicationBuilder UseExceptionHandlingMiddleware(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<ExceptionHandlingMiddleware>();
    }
}
