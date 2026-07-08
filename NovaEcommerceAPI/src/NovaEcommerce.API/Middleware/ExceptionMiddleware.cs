using Microsoft.AspNetCore.Http;
using System.Net;
using System.Text.Json;

namespace NovaEcommerce.API.Middleware;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (UnauthorizedAccessException ex)
        {
            await WriteResponse(
                context,
                HttpStatusCode.Unauthorized,
                ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            await WriteResponse(
                context,
                HttpStatusCode.Conflict,
                ex.Message);
        }
        catch (KeyNotFoundException ex)
        {
            await WriteResponse(
                context,
                HttpStatusCode.NotFound,
                ex.Message);
        }
        catch (Exception ex)
        {
            await WriteResponse(
                context,
                HttpStatusCode.BadRequest,
                ex.Message);
        }
    }

    private static async Task WriteResponse(
        HttpContext context,
        HttpStatusCode statusCode,
        string message)
    {
        context.Response.ContentType = "application/json";

        context.Response.StatusCode = (int)statusCode;

        var result = JsonSerializer.Serialize(new
        {
            statusCode = (int)statusCode,
            message
        });

        await context.Response.WriteAsync(result);
    }
}