using Microsoft.AspNetCore.Http;
using System;
using System.Net;
using System.Threading.Tasks;
using DevEdu.API.Configuration.ExceptionResponses;
using DevEdu.Business.Exceptions;
using Newtonsoft.Json;

namespace DevEdu.API.Configuration
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private const string MessageAuthorization = "Authorization exception";
        private const string MessageValidation = "Validation exception";
        private const string MessageEntity = "Entity not found exception";
        private const int AuthorizationCode = 1000;
        private const int ValidationCode = 1001;
        private const int EntityCode = 1002;

        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next.Invoke(context);
            }
            catch (AuthorizationException ex)
            {
                await HandlerExceptionMessageAsync(context, ex, AuthorizationCode, MessageAuthorization);
            }
            catch (ValidationException ex) //422
            {
                await HandleValidationExceptionMessageAsync(context, ex);
            }
            catch (EntityNotFoundException ex) //404
            {
                await HandlerExceptionMessageAsync(context, ex, EntityCode, MessageEntity);
            }
            catch (Exception ex)
            {
                await HandleExceptionMessageAsync(context, ex);
            }
        }

        private static Task HandlerExceptionMessageAsync(HttpContext context, Exception exception, int code, string message)
        {
            context.Response.ContentType = "application/json";
            var result = JsonConvert.SerializeObject(
                new ExceptionResponse
                {
                    Code = code,
                    Message = message,
                    Description = exception.Message
                }
            );
            context.Response.StatusCode = (int)HttpStatusCode.Forbidden;
            return context.Response.WriteAsync(result);
        }

        private static Task HandleValidationExceptionMessageAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            var result = JsonConvert.SerializeObject(
                new ValidationExceptionResponse
                {
                    //todo implement this
                }
            );
            context.Response.StatusCode = (int)HttpStatusCode.UnprocessableEntity;
            return context.Response.WriteAsync(result);
        }

        private static Task HandleExceptionMessageAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            var result = JsonConvert.SerializeObject(new
            {
                code = 1003,
                message = "Unknown error",
                description = exception.Message
            });
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            return context.Response.WriteAsync(result);
        }
    }
}