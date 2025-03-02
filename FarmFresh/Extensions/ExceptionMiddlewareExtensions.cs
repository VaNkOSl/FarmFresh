﻿using LoggerService.Contacts;
using LoggerService.Exceptions;
using Microsoft.AspNetCore.Diagnostics;

namespace FarmFresh.Extensions;

public static class ExceptionMiddlewareExtensions
{
    public static void ConfigureExceptionHandler(this WebApplication app, ILoggerManager logger)
    {
        app.UseExceptionHandler(appError =>
        {
            appError.Run(async context =>
            {
                var contextFeature = context.Features.Get<IExceptionHandlerFeature>();

                if (contextFeature != null)
                {
                    var statusCode = contextFeature.Error switch
                    {
                        NotFoundException => StatusCodes.Status404NotFound,
                        BadRequestException => StatusCodes.Status400BadRequest,
                        InternalServiceError => StatusCodes.Status500InternalServerError,
                        _ => StatusCodes.Status500InternalServerError
                    };

                    logger.LogError($"[{DateTime.UtcNow}] Error occurred: {contextFeature.Error}");

                    var errorRoute = statusCode switch
                    {
                        StatusCodes.Status404NotFound => "/api/error/notfound",
                        StatusCodes.Status400BadRequest => "/api/error/BadRequest",
                        StatusCodes.Status500InternalServerError => "/api/error/internalserverError",
                        _ => "/Error/General"
                    };

                    context.Response.Redirect(errorRoute);
                }
            });
        });
    }
}
