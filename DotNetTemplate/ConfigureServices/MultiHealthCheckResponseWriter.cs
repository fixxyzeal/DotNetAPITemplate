using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.Text.Json;

namespace DotNetTemplate.ConfigureServices
{
    public static class MultiHealthCheckResponseWriter
    {
        public static Task WriteResponse(HttpContext httpContext, HealthReport result)
        {
            var response = new
            {
                status = result.Status.ToString(),
                results = result.Entries.Select(pair =>
                    new
                    {
                        name = pair.Key,
                        status = pair.Value.Status.ToString(),
                        description = pair.Value.Description,
                        data = pair.Value.Data,
                    }).ToArray()
            };

            httpContext.Response.ContentType = "application/json";
            string responseJson = JsonSerializer.Serialize(response, new JsonSerializerOptions
            {
                WriteIndented = true
            });

            return httpContext.Response.WriteAsync(responseJson);
        }
    }
}