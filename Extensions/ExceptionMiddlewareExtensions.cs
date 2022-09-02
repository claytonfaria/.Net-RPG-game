using System.Net;
using dotnet_rpg.Models;
using Microsoft.AspNetCore.Diagnostics;

namespace dotnet_rpg.Extensions;

public static class ExceptionMiddlewareExtensions
{
    public static void ConfigureExceptionHandler(this WebApplication app)
    {
        if (app.Environment.IsDevelopment()) return;
        
        var logger = app.Services.GetRequiredService<ILogger<Program>>();

        app.UseExceptionHandler(appError =>
        {
            appError.Run(async context =>
            {
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                context.Response.ContentType = "application/json";
                var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                if (contextFeature != null)
                {
                    logger.LogError("Something went wrong: {ContextFeatureError}", contextFeature.Error);
                    await context.Response.WriteAsync(new ErrorResponse()
                    {
                        StatusCode = context.Response.StatusCode,
                        Message = "Internal Server Error."
                    }.ToString());
                }
            });
        });
    }
}