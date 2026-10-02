using Banking.API.Application.Exceptions;
using Banking.API.Models.Exceptions;
using FluentValidation;
using System.Net;
using System.Text.Json;

namespace Banking.Api.Middleware;

public class GlobalExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionMiddleware> _logger;

    public GlobalExceptionMiddleware(
        RequestDelegate next,
        ILogger<GlobalExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(
        HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (ValidationException ex)
        {
            await HandleValidationExceptionAsync(
                context,
                ex);
        }
        catch (AccountNotFoundException ex)
        {
            await WriteErrorAsync(
                context,
                HttpStatusCode.NotFound,
                ex.Message);
        }
        catch (DomainException ex)
        {
            await WriteErrorAsync(
                context,
                HttpStatusCode.UnprocessableEntity,
                ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Erro não tratado.");

            await WriteErrorAsync(
                context,
                HttpStatusCode.InternalServerError,
                "Ocorreu um erro interno.");
        }
    }

    private static async Task HandleValidationExceptionAsync(
        HttpContext context,
        ValidationException exception)
    {
        context.Response.StatusCode =
            (int)HttpStatusCode.BadRequest;

        context.Response.ContentType =
            "application/json";

        var errors = exception.Errors
            .Select(error => new
            {
                error.PropertyName,
                error.ErrorMessage
            });

        var response = new
        {
            errors
        };

        await context.Response.WriteAsync(
            JsonSerializer.Serialize(response));
    }

    private static async Task WriteErrorAsync(
        HttpContext context,
        HttpStatusCode statusCode,
        string message)
    {
        context.Response.StatusCode =
            (int)statusCode;

        context.Response.ContentType =
            "application/json";

        var response = new
        {
            error = message
        };

        await context.Response.WriteAsync(
            JsonSerializer.Serialize(response));
    }
}