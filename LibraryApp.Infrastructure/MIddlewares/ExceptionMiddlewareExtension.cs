using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Text;

namespace LibraryApp.Infrastructure.MIddlewares
{
    public static class ExceptionMiddlewareExtension
    {
        /// <summary>
        /// Adds <see cref="ExceptionMiddleware"/> to your application pipeline to handle all unhandled exceptions.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <returns></returns>
        public static IApplicationBuilder UseExceptionMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ExceptionMiddleware>();
        }
    }
}
