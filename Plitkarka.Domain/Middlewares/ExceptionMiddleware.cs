using System.Net;
using Amazon.Runtime;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Plitkarka.Commons.Exceptions;

namespace Plitkarka.Domain.Middlewares;

public class ExceptionMiddleware 
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionMiddleware> _logger;

    public ExceptionMiddleware(
        RequestDelegate next,
        ILogger<ExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
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
        catch (Exception ex) when (ex is S3ServiceException)
        {
            HandleS3ServiceException(httpContext);
        }
        catch (ValidationException ex)
        {
            await HandleValidationException(httpContext, ex);
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
    private void HandleS3ServiceException(HttpContext httpContext)
    {
        httpContext.Response.StatusCode = (int) HttpStatusCode.InternalServerError;
    }

    private async Task HandleValidationException(HttpContext httpContext, ValidationException ex)
    {
        if (ex.ParamName != null)
        {
            // Adding the name of param (Email/Login) that already exist in database 
            httpContext.Response.Headers.Add("FailedParam", ex.ParamName);
        }

        httpContext.Response.StatusCode = (int) HttpStatusCode.BadRequest;
        httpContext.Response.ContentType = "text/plain; charset=utf-8";
        await httpContext.Response.WriteAsync(ex.Message);
    }

    private void HandleException(HttpContext httpContext)
    {
        httpContext.Response.StatusCode = (int) HttpStatusCode.InternalServerError;
    }
}
