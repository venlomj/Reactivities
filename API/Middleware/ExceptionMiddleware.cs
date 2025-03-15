using System.Text.Json;
using Application.Core;
using FluentValidation;

namespace API.Middleware
{
    public class ExceptionMiddleware(ILogger<ExceptionMiddleware> logger,
        IHostEnvironment env) : IMiddleware
    {
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                // Attempts to invoke the next middleware component in the pipeline
                await next(context);
            }
            catch (ValidationException ex)
            {
                // Catches validation errors and handles them separately
                await HandleValidationException(context, ex);
            }
            catch (Exception ex)
            {
                await HandleException(context, ex);
            }
        }

        private async Task HandleException(HttpContext context, Exception ex)
        {
            logger.LogError(ex, ex.Message);
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;

            var response = env.IsDevelopment()
                ? new AppException(context.Response.StatusCode, ex.Message, ex.StackTrace)
                : new AppException(context.Response.StatusCode, ex.Message, null);

            var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

            var json = JsonSerializer.Serialize(response, options);

            await context.Response.WriteAsync(json);
        }

        private static async Task HandleValidationException(HttpContext context, ValidationException ex)
        {
            var validationErrors = new Dictionary<string, string[]>();

            if (ex.Errors is not null)
            {
                foreach (var error in ex.Errors)
                {
                    if (validationErrors.TryGetValue(error.PropertyName, out var existingErrors))
                    {
                        //validationErrors[error.PropertyName] = existingErrors.Append(error.ErrorMessage).ToArray(); // Old way
                        // Adds the error message to the existing list of errors for this field
                        validationErrors[error.PropertyName] = [.. existingErrors, error.ErrorMessage];
                    }
                    else
                    {
                        // Creates a new list of error messages for this field
                        validationErrors[error.PropertyName] = [error.ErrorMessage];
                    }
                }
            }

            // Sets the HTTP status code to 400 (Bad Request)
            context.Response.StatusCode = StatusCodes.Status400BadRequest;

            // Creates a standardized validation error response
            var validationProblemDetails = new HttpValidationProblemDetails(validationErrors)
            {
                Status = StatusCodes.Status400BadRequest,
                Type = "ValidationFailure",
                Title = "Validation error",
                Detail = "One or more validation errors have occurred"
            };

            // Sends the validation errors as a JSON response
            await context.Response.WriteAsJsonAsync(validationProblemDetails);
        }
    }
}
