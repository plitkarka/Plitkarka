using System.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Plitkarka.Commons.Exceptions;

namespace Plitkarka.Domain.Middlewares;

public class ExceptionMiddleware 
{
    private readonly RequestDelegate _next;
    private readonly ILogger _logger;

    public ExceptionMiddleware(
        RequestDelegate next,
        ILoggerFactory loggerFactory)
    {
        _next = next;
        _logger = loggerFactory.CreateLogger<ExceptionMiddleware>();
    }

    public async Task InvokeAsync(HttpContext httpContext)
    {
        try
        {
            await _next(httpContext);
        }
        catch(Exception ex) when (ex is MySqlException)
        {
            HandleMySqlException(httpContext);
        }
        catch (ValidationException ex)
        {
            HandleValidationException(httpContext, ex);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            HandleException(httpContext);
        }
    }

    private void HandleMySqlException(HttpContext httpContext)
    {
        httpContext.Response.StatusCode = (int) HttpStatusCode.InternalServerError;
    }

    private void HandleValidationException(HttpContext httpContext, ValidationException ex)
    {
        if (ex.ParamName != null)
        {
            // Adding the name of param (Email/Login) that already exist in database 
            httpContext.Response.Headers.Add("FailedParam", ex.ParamName);
        }

        httpContext.Response.StatusCode = (int) HttpStatusCode.BadRequest;
    }

    private void HandleException(HttpContext httpContext)
    {
        httpContext.Response.StatusCode = (int) HttpStatusCode.InternalServerError;
    }
}
