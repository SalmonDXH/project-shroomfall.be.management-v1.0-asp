using Application.Interface.Utility;
using Contract.DTO.Abstraction;
using Domain.DomainException;
using ResponseCode;
using System.Text.Json;

namespace API.Middleware
{
    public class GlobalExceptionHandler
    {
        #region Attributes
        private readonly RequestDelegate requestDelegate;
        private readonly ILogger<GlobalExceptionHandler> logger;

        private static readonly string defaultErrorCode = APICode.GlobalExceptionHandlerCode.UnexpectedError;
        #endregion

        #region Properties
        #endregion

        public GlobalExceptionHandler(
            RequestDelegate requestDelegate,
            ILogger<GlobalExceptionHandler> logger
            )
        {
            this.requestDelegate = requestDelegate;
            this.logger = logger;
        }

        #region Methods
        public async Task InvokeAsync(
            HttpContext context)
        {
            try
            {
                await requestDelegate(context);
            }
            catch (Exception ex)
            {
                logger.LogError(
                    ex,
                    "Unhandled exception occurred. Path: {Path}, Method: {Method}, TraceId: {TraceId}",
                    context.Request.Path,
                    context.Request.Method,
                    context.TraceIdentifier);

                await HandleExceptionAsync(context, ex);
            }
        }

        private static async Task HandleExceptionAsync(
            HttpContext context,
            Exception exception)
        {
            context.Response.ContentType = "application/json";

            var statusCode = StatusCodes.Status500InternalServerError;
            var type = "Unexpected Internal Error";
            var message = "An unexpected error occurred. Please try again later.";
            string errorCode = defaultErrorCode;
            string? details = null;

            switch (exception)
            {
                case BadRequest badRequest:
                    statusCode = StatusCodes.Status400BadRequest;
                    type = "Bad Request";
                    message = badRequest.Message;
                    errorCode = badRequest.Code;
                    break;

                case NotFound notFound:
                    statusCode = StatusCodes.Status404NotFound;
                    type = "Not Found";
                    message = notFound.Message;
                    errorCode = notFound.Code;
                    break;

                case Unauthorized unauthorized:
                    statusCode = StatusCodes.Status401Unauthorized;
                    type = "Unauthorized";
                    message = unauthorized.Message;
                    errorCode = unauthorized.Code;
                    break;

                case InternalException internalEx:
                    statusCode = StatusCodes.Status500InternalServerError;
                    type = "Internal Server Error";
                    message = internalEx.Message;
                    errorCode = internalEx.Code;
                    break;

                default:
                    details = exception.ToString();
                    break;
            }

            context.Response.StatusCode = statusCode;

            // Use the explicit contract DTO instead of an anonymous type
            var response = new ApiErrorDTO
            {
                Type = type,
                Code = errorCode,
                Message = message,
                Details = details
            };

            var json = JsonSerializer.Serialize(response, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true
            });

            await context.Response.WriteAsync(json);
        }
        #endregion
    }
}