using System.Net;

using Microsoft.AspNetCore.Http;

namespace Downhealth.Antigravity.Infrastructure.Middleware;

public class UnauthorizedAccessExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;

    public UnauthorizedAccessExceptionHandlingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (UnauthorizedAccessException ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private Task HandleExceptionAsync(HttpContext context, UnauthorizedAccessException ex)
    {
        context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
        context.Response.ContentType = "application/json";

        var result = new
        {
            error = ex.Message
        };

        return context.Response.WriteAsync(System.Text.Json.JsonSerializer.Serialize(result));
    }
}