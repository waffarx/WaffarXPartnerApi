using Microsoft.AspNetCore.Mvc.Filters;
using System.IO.Compression;
public class CompressResponseAttribute : ActionFilterAttribute
{
    public override async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
    {
        // Save original body stream
        var originalBodyStream = context.HttpContext.Response.Body;
        try
        {
            // Create a temporary memory stream
            using var responseBodyStream = new MemoryStream();
            // Replace the response body with our memory stream
            context.HttpContext.Response.Body = responseBodyStream;
            // Execute the rest of the pipeline
            await next();
            // Check if client accepts gzip
            var acceptEncoding = context.HttpContext.Request.Headers["Accept-Encoding"].ToString().ToLowerInvariant();
            var canGzip = !string.IsNullOrEmpty(acceptEncoding) && acceptEncoding.Contains("gzip");
            // Reset position to read from the beginning
            responseBodyStream.Seek(0, SeekOrigin.Begin);
            // Only compress if the client accepts it
            if (canGzip)
            {
                context.HttpContext.Response.Headers["Content-Encoding"] = "gzip";
                // Create a gzip stream that writes to the original response stream
                var gzipStream = new GZipStream(originalBodyStream, CompressionLevel.Fastest, leaveOpen: true);
                try
                {
                    // Use async copy method
                    await responseBodyStream.CopyToAsync(gzipStream);
                    await gzipStream.FlushAsync();
                }
                finally
                {
                    // Dispose the gzip stream asynchronously
                    await gzipStream.DisposeAsync();
                }
            }
            else
            {
                // Use async copy method for uncompressed response
                await responseBodyStream.CopyToAsync(originalBodyStream);
            }
        }
        finally
        {
            // Always restore the original stream
            context.HttpContext.Response.Body = originalBodyStream;
        }
    }
}
