using System.Net.Mime;
using System.Text.Json;

namespace MP.API.Middleware;

public class ExceptionsHandlingMiddleware(
    RequestDelegate next,
    ILogger<ExceptionsHandlingMiddleware> logger)
{
    private static readonly JsonSerializerOptions JsonSerializerOptions = new()
        { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            await HandleException(context, ex);
        }
    }

    private async Task HandleException(HttpContext context, Exception ex)
    {
        var badResponse = new { Error = ex.Message };

        logger.LogError(ex, "Внутренняя ошибка сервера");

        context.Response.Clear();
        context.Response.StatusCode = StatusCodes.Status500InternalServerError;
        context.Response.ContentType = MediaTypeNames.Application.Json;

        var badResponseJson = JsonSerializer.Serialize(badResponse, JsonSerializerOptions);

        await context.Response.WriteAsync(badResponseJson);
    }
}