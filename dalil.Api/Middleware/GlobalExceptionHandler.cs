using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using static dalil.Core.Exceptions.AppExceptions;

namespace DalilAcademy.API.Middleware;

// Lecture 6: one place turns every exception into the right status code + ProblemDetails
public class GlobalExceptionHandler(
    IProblemDetailsService problemDetails,
    ILogger<GlobalExceptionHandler> logger) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext context, Exception exception, CancellationToken ct)
    {
        var (status, title) = exception switch
        {
            ValidationException => (StatusCodes.Status400BadRequest, "Validation failed"),
            NotFoundException => (StatusCodes.Status404NotFound, "Resource not found"),
            ConflictException => (StatusCodes.Status409Conflict, "Conflict"),
            UnauthorizedException => (StatusCodes.Status401Unauthorized, "Unauthorized"),
            InvalidFileException => (400, "Invalid file"),
            _ => (StatusCodes.Status500InternalServerError, "An unexpected error occurred")
        };

        // 500s are logged — and never leak details to the client
        if (status == StatusCodes.Status500InternalServerError)
            logger.LogError(exception, "Unhandled exception");

        context.Response.StatusCode = status;

        var problem = new ProblemDetails
        {
            Status = status,
            Title = title,
            Detail = exception is NotFoundException or ConflictException or UnauthorizedException or InvalidFileException
            ? exception.Message
            : null
        };

        if (exception is ValidationException ve)
        {
            problem.Extensions["errors"] = ve.Errors
                .GroupBy(e => e.PropertyName)
                .ToDictionary(
                    g => g.Key,
                    g => g.Select(e => e.ErrorMessage).ToArray());
        }

        return await problemDetails.TryWriteAsync(new()
        {
            HttpContext = context,
            ProblemDetails = problem,
            Exception = exception
        });
    }
}
