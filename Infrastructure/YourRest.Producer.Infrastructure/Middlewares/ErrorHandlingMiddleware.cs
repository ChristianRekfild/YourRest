using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;
using System.Text.Json;
using YourRest.Application.Exceptions;

namespace YourRest.Producer.Infrastructure.Middlewares;
public class ErrorHandlingMiddleware
{
    private readonly RequestDelegate _next;

    public ErrorHandlingMiddleware(RequestDelegate next)
    {
        _next = next;
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

    private static Task HandleExceptionAsync(HttpContext context, Exception ex)
    {
        int statusCode;
        string message = ex.Message;

        switch (ex)
        {
            case EntityNotFoundException:
                statusCode = StatusCodes.Status404NotFound;
                break;
            case InvalidParameterException:
            case MissingParameterException:
                statusCode = StatusCodes.Status400BadRequest;
                break;
            case ValidationException:
                statusCode = StatusCodes.Status422UnprocessableEntity;
                break;
            case EntityConflictException:
                statusCode = StatusCodes.Status409Conflict;
                break;
            case UnexpectedErrorException:
                statusCode = StatusCodes.Status500InternalServerError;
                break;
            default:
                statusCode = StatusCodes.Status500InternalServerError;
                message = "Internal server error";
                break;
        }

        context.Response.StatusCode = statusCode;
        context.Response.ContentType = "application/json";

        var response = new
        {
            message = message
        };
        var jsonResponse = JsonSerializer.Serialize(response);

        return context.Response.WriteAsync(jsonResponse);
    }
}
