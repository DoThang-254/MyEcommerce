using System.Net;
using System.Text.Json;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Shared.Domain.Exceptions;

namespace Shared.API.Middlewares;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
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
            _logger.LogError(ex, "An unhandled exception occurred: {Message}", ex.Message);
            await HandleExceptionAsync(context, ex);
        }
    }

    private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";

        var (statusCode, title, message) = exception switch
        {
            // Bắt lỗi NotFound
            NotFoundException notFoundEx =>
                (HttpStatusCode.NotFound, notFoundEx.Title, notFoundEx.Message),

            // BẮT THÊM: Các lỗi DomainException khác (ví dụ: OutOfStock, InvalidAction...) -> Trả về 400
            DomainException domainEx =>
                (HttpStatusCode.BadRequest, domainEx.Title ?? "Bad Request", domainEx.Message),

            ValidationException validationEx =>
                (HttpStatusCode.BadRequest, "Validation Error", string.Join("; ", validationEx.Errors.Select(e => e.ErrorMessage))),

            ArgumentException argEx =>
                (HttpStatusCode.BadRequest, "Invalid Argument", argEx.Message),

            _ =>
                (HttpStatusCode.InternalServerError, "Server Error", "An unexpected error occurred.")
        };

        context.Response.StatusCode = (int)statusCode;

        var response = new
        {
            StatusCode = (int)statusCode,
            Title = title,
            Message = message,
            Timestamp = DateTime.UtcNow
        };

        var json = JsonSerializer.Serialize(response, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });

        await context.Response.WriteAsync(json);
    }
}
