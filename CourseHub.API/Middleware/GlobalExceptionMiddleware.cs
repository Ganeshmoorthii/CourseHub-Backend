using System;
using System.Net;
using System.Threading.Tasks;
using CourseHub.Application.Exceptions;
using CourseHub.API.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace CourseHub.API.Middleware
{
    public class GlobalExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionMiddleware> _logger;

        public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
        {
            _next = next ?? throw new ArgumentNullException(nameof(next));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled exception occurred while processing the request.");

                if (context.Response.HasStarted)
                {
                    _logger.LogWarning("Response has already started, rethrowing exception.");
                    throw;
                }

                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            context.Response.ContentType = "application/json";

            var statusCode = ex switch
            {
                NotFoundException => HttpStatusCode.NotFound,
                ValidationException => HttpStatusCode.BadRequest,
                ConflictException => HttpStatusCode.Conflict,
                UnauthorizedAccessException => HttpStatusCode.Unauthorized,
                _ => HttpStatusCode.InternalServerError
            };

            context.Response.StatusCode = (int)statusCode;

            var errorResponse = new ErrorResponse
            {
                Status = context.Response.StatusCode,
                Message = ex?.Message ?? "An unexpected error occurred.",
                TraceId = context.TraceIdentifier
            };

            return context.Response.WriteAsJsonAsync(errorResponse);
        }
    }
}
