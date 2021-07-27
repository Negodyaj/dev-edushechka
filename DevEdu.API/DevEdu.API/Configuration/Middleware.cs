using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace DevEdu.API.Configuration
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class Middleware
    {
        private readonly RequestDelegate _next;

        public Middleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await  _next.Invoke(context);
            }
            catch (Exception ex)
            {
                await  WriteResponse(context,ex).ConfigureAwait(false);
            }
        }

        public static async Task WriteResponse(HttpContext httpContext, Exception exception)
        {
            httpContext.Response.ContentType = "application/json";
            var errorMessage = exception.Message;
            int statusCode = (int)HttpStatusCode.InternalServerError;

            var problem = new ProblemDetails
            {
                Status = statusCode,
                Title = errorMessage,
            };

            var stream = httpContext.Response.Body;
            await  JsonSerializer.SerializeAsync(stream, problem);
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class MiddlewareExtensions
    {
        public static IApplicationBuilder UseMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<Middleware>();
        }
    }
}