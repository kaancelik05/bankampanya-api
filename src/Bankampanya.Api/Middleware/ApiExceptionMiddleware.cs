using System.Net;
using System.Text.Json;
using Bankampanya.Application.Common.Exceptions;
using FluentValidation;
using Microsoft.AspNetCore.Http;

namespace Bankampanya.Api.Middleware;

public sealed class ApiExceptionMiddleware(RequestDelegate next, ILogger<ApiExceptionMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (ValidationException ex)
        {
            await WriteProblemDetailsAsync(
                context,
                HttpStatusCode.BadRequest,
                "Validation failed",
                ex.Message,
                ex.Errors.GroupBy(x => x.PropertyName).ToDictionary(
                    group => group.Key,
                    group => group.Select(error => error.ErrorMessage).ToArray()),
                logger,
                ex);
        }
        catch (EntityNotFoundException ex)
        {
            await WriteProblemDetailsAsync(
                context,
                HttpStatusCode.NotFound,
                "Resource not found",
                ex.Message,
                null,
                logger,
                ex);
        }
        catch (DomainValidationException ex)
        {
            await WriteProblemDetailsAsync(
                context,
                HttpStatusCode.BadRequest,
                "Domain validation failed",
                ex.Message,
                null,
                logger,
                ex);
        }
        catch (UnauthorizedException ex)
        {
            await WriteProblemDetailsAsync(
                context,
                HttpStatusCode.Unauthorized,
                "Unauthorized",
                ex.Message,
                null,
                logger,
                ex);
        }
        catch (Exception ex)
        {
            await WriteProblemDetailsAsync(
                context,
                HttpStatusCode.InternalServerError,
                "Unexpected error",
                "An unexpected error occurred.",
                null,
                logger,
                ex);
        }
    }

    private static async Task WriteProblemDetailsAsync(
        HttpContext context,
        HttpStatusCode statusCode,
        string title,
        string detail,
        IDictionary<string, string[]>? errors,
        ILogger logger,
        Exception exception)
    {
        logger.LogError(exception, "API request failed with status code {StatusCode}", (int)statusCode);

        context.Response.StatusCode = (int)statusCode;
        context.Response.ContentType = "application/problem+json";

        var payload = new
        {
            title,
            status = (int)statusCode,
            detail,
            errors,
            traceId = context.TraceIdentifier,
        };

        await context.Response.WriteAsync(JsonSerializer.Serialize(payload));
    }
}
