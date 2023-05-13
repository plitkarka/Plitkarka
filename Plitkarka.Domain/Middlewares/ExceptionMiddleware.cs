using System.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Plitkarka.Commons.Exceptions;
using Plitkarka.Commons.Logger;

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
        httpContext.Response.ContentType = "application/json";
        
        try
        {
            await _next(httpContext);
        }
        catch(MySqlException ex)
        {
            _logger.LogDatabaseError(ex.Message);
            await HandleMySqlException(httpContext);
        }
        catch (S3ServiceException ex)
        {
            _logger.LogS3Error(ex.Message);
            await HandleS3ServiceException(httpContext);
        }
        catch (EmailServiceException ex)
        {
            _logger.LogEmailSendingError(ex.Message);
            await HandleEmailServiceException(httpContext);
        }
        catch (ValidationException ex)
        {
            await HandleValidationException(httpContext, ex);
        }
        catch (UnauthorizedUserException ex)
        {
            await HandleUnathorizedUserException(httpContext, ex);
        }
        catch (AuthorizationErrorException ex)
        {
            await HandleAuthorizationErrorException(httpContext, ex);
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogArgumentNullException(ex.Message);
            await HandleException(httpContext, ex);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            await HandleException(httpContext, ex);
        }
    }

    #region System exceptions 
    private async Task HandleMySqlException(HttpContext httpContext)
    {
        httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
        httpContext.Response.ContentType = textPlain;
        await httpContext.Response.WriteAsync("Error working with database");
    }
    private async Task HandleS3ServiceException(HttpContext httpContext)
    {
        httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
        httpContext.Response.ContentType = textPlain;
        await httpContext.Response.WriteAsync("Error working with images");
    }
    private async Task HandleEmailServiceException(HttpContext httpContext)
    {
        httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
        httpContext.Response.ContentType = textPlain;
        await httpContext.Response.WriteAsync("Error sending emails");
    }

    #endregion

    #region Request exceptions 

    private async Task HandleAuthorizationErrorException(HttpContext httpContext, AuthorizationErrorException ex)
    {
        httpContext.Response.StatusCode = (int)HttpStatusCode.Forbidden;
        httpContext.Response.ContentType = textPlain;
        await httpContext.Response.WriteAsync(ex.Message);
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

    #endregion

    private async Task HandleException(HttpContext httpContext, Exception ex)
    {
        _logger.LogError(ex.Message);
        httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
        httpContext.Response.ContentType = textPlain;
        await httpContext.Response.WriteAsync("Internal server error");
    }
}
