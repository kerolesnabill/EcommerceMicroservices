using BuildingBlocks.Exceptions;
using FluentValidation;
using Grpc.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Net;

namespace BuildingBlocks.Middlewares;

public class ErrorHandlingMiddleware(ILogger<ErrorHandlingMiddleware> logger) : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next.Invoke(context);
        }
        catch (UnauthorizedAccessException ex)
        {
            logger.LogWarning(ex.Message);
            context.Response.StatusCode = 401;
            await context.Response.WriteAsJsonAsync(new { error = ex.Message });
        }
        catch (ConflictException ex)
        {
            logger.LogWarning(ex.Message);
            context.Response.StatusCode = 409;
            await context.Response.WriteAsJsonAsync(new { error = ex.Message });
        }
        catch (InsufficientStockException ex)
        {
            logger.LogWarning(ex.Message);
            context.Response.StatusCode = 409;
            await context.Response.WriteAsJsonAsync(new { error = ex.Message });
        }
        catch (ForbiddenException ex)
        {
            logger.LogWarning(ex.Message);
            context.Response.StatusCode = 403;
            await context.Response.WriteAsJsonAsync(new { error = ex.Message });
        }

        catch (NotFoundException ex)
        {
            logger.LogWarning(ex.Message);
            context.Response.StatusCode = 404;
            await context.Response.WriteAsJsonAsync(new { error = ex.Message });
        }
        catch (BadHttpRequestException ex)
        {
            logger.LogWarning(ex.Message);
            context.Response.StatusCode = 400;
            await context.Response.WriteAsJsonAsync(new { error = ex.Message });
        }
        catch (ValidationException ex)
        {
           
            logger.LogWarning(ex.Message);
            context.Response.StatusCode = 400;
            await context.Response.WriteAsJsonAsync(ex.Errors.Select(e =>
                    new { message = e.ErrorMessage, field = e.PropertyName }));
        }
        catch (RpcException ex)
        {
            var status = ex.StatusCode switch
            {
                StatusCode.NotFound => HttpStatusCode.NotFound,
                _ => HttpStatusCode.BadRequest,
            };

            logger.LogWarning(ex.Message);
            context.Response.StatusCode = (int)status;
            await context.Response.WriteAsJsonAsync(new { error = ex.Status.Detail ?? ex.Message });
        }
        catch (Exception ex)
        {
            logger.LogError(ex.Message, ex);

            context.Response.StatusCode = 500;
            await context.Response.WriteAsJsonAsync(new { error = "Something went wrong" });
        }
    }
}
