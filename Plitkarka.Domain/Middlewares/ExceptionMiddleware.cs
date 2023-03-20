using System.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Plitkarka.Commons.Exceptions;

namespace Plitkarka.Domain.Middlewares;

public class ExceptionMiddleware 
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionMiddleware> _logger;

    private readonly string textPlain = "text/plain; charset=utf-8";

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
        catch(MySqlException ex) 
        {
            await HandleMySqlException(httpContext, ex);
        }
        catch (Exception ex) when (ex is S3ServiceException)
        {
            HandleS3ServiceException(httpContext);
        }
        catch (Exception ex) when (ex is EmailServiceException)
        {
            HandleEmailServiceException(httpContext);
        }
        catch (ValidationException ex)
        {
            await HandleValidationException(httpContext, ex);
        }
        catch (InvalidTokenException ex)
        {
            await HandleInvalidTokenException(httpContext, ex);
        }
        catch (UnauthorizedUserException ex)
        {
            await HandleUnathorizedUserException(httpContext, ex);
        }
        catch (AuthorizationErrorException ex)
        {
            await HandleAuthorizationErrorException(httpContext, ex);
        }
        catch (Exception ex)
        {
            await HandleException(httpContext, ex);
        }
    }

    private async Task HandleAuthorizationErrorException(HttpContext httpContext, AuthorizationErrorException ex)
    {
        httpContext.Response.StatusCode = (int)HttpStatusCode.Forbidden;
        httpContext.Response.ContentType = textPlain;
        await httpContext.Response.WriteAsync(ex.Message);
    }

    private async Task HandleInvalidTokenException(HttpContext httpContext, InvalidTokenException ex)
    {
        httpContext.Response.StatusCode = (int) HttpStatusCode.BadRequest;
        httpContext.Response.ContentType = textPlain;
        await httpContext.Response.WriteAsync(ex.Message);
    }

    private async Task HandleMySqlException(HttpContext httpContext, MySqlException ex)
    {
        _logger.LogError(ex.Message);
        httpContext.Response.StatusCode = (int) HttpStatusCode.InternalServerError;
        httpContext.Response.ContentType = textPlain;
        await httpContext.Response.WriteAsync("Error happened while working with database");
    }
    private void HandleS3ServiceException(HttpContext httpContext)
    {
        httpContext.Response.StatusCode = (int) HttpStatusCode.InternalServerError;
    }
    private void HandleEmailServiceException(HttpContext httpContext)
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
        httpContext.Response.ContentType = textPlain;
        await httpContext.Response.WriteAsync(ex.Message);
    }

    private async Task HandleUnathorizedUserException(HttpContext httpContext, UnauthorizedUserException ex)
    {
        httpContext.Response.StatusCode = (int) HttpStatusCode.Unauthorized;
        httpContext.Response.ContentType = textPlain;
        await httpContext.Response.WriteAsync(ex.Message);
    }

    private async Task HandleException(HttpContext httpContext, Exception ex)
    {
        _logger.LogError(ex.Message);
        httpContext.Response.StatusCode = (int) HttpStatusCode.InternalServerError;
        httpContext.Response.ContentType = textPlain;
        await httpContext.Response.WriteAsync("Internal server error");
    }
}
