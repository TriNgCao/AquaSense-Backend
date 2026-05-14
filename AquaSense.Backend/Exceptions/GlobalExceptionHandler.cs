using System.Text.Json;
using AquaSense.Backend.Models.Response;

namespace AquaSense.Backend.Exceptions;

public class GlobalExceptionHandler
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionHandler> _logger;

    public GlobalExceptionHandler(RequestDelegate next, ILogger<GlobalExceptionHandler> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception ex)
    {
        var statusCode = StatusCodes.Status500InternalServerError;
        var message = "Internal server error";

        if (ex is AppException appException)
        {
            statusCode = appException.StatusCode;
            message = appException.Message;
        }

        _logger.LogError(ex, "Unhandled exception: {Message}", ex.Message);

        context.Response.StatusCode = statusCode;
        context.Response.ContentType = "application/json";

        var payload = ApiResponse<string>.Fail(message);
        var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
        await context.Response.WriteAsync(JsonSerializer.Serialize(payload, options));
    }
}
