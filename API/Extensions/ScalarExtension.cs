using Microsoft.OpenApi.Models;
using Scalar.AspNetCore;

namespace API.Extensions
{
    /// <summary>
    /// Provides a method to map Scalar API documentation for the Reactivities REST API.
    /// </summary>
    public static class ScalarExtension
    {
        /// <summary>
        /// Maps the Scalar API Reference UI for the Reactivities project.
        /// </summary>
        /// <param name="app">The web application instance.</param>
        public static void AddScalarDocumentation(this WebApplication app)
        {
            app.MapScalarApiReference(options =>
            {
                options.Title = "Reactivities API";
                options.Theme = ScalarTheme.DeepSpace;
                options.DefaultHttpClient = new(ScalarTarget.CSharp, ScalarClient.HttpClient);
            });
        }

        public static IServiceCollection AddScalarTransformer(this IServiceCollection services)
        {
            services.AddOpenApi(options =>
            {
                options.AddDocumentTransformer((documentation, context, _) =>
                {
                    documentation.Info = new OpenApiInfo
                    {
                        Title = "Reactivities API",
                        Version = "v1",
                        Description = "The Reactivities API provides endpoints for user authentication and activities management, including JWT and cookie-based authentication.", // Description updated for clarity
                        Contact = new OpenApiContact
                        {
                            Name = "venlomj",
                            Email = "venlo.mj@hotmail.nl"
                        }
                    };

                    return Task.CompletedTask;
                });
            });
            return services;
        }
    }
}