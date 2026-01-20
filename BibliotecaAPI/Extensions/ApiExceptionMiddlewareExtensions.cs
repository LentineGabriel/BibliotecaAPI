#region USINGS
using BibliotecaAPI.Models;
using Microsoft.AspNetCore.Diagnostics;
#endregion

namespace BibliotecaAPI.Extensions;
public static class ApiExceptionMiddlewareExtensions
{
    public static void ConfigureApiExceptionMiddleware(this WebApplication app)
    {
        app.UseExceptionHandler(appError =>
        {
            appError.Run(async context =>
            {
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                context.Response.ContentType = "application/json";

                var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                if(contextFeature != null)
                {
                    await context.Response.WriteAsync(new ErrosDetalhados()
                    {
                        StatusCode = context.Response.StatusCode,
                        Message = contextFeature.Error.Message,
                        Trace = contextFeature.Error.StackTrace
                    }.ToString());
                }
            });
        });
    }
}
