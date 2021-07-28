using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Net;

namespace DevEdu.API.Configuration.ExceptionResponses
{
    public class ValidationExceptionResponse : Exception
    {
        public int StatusCode { get; private set; }
        public string ErrorMessage { get; private set; }

        public ValidationExceptionResponse(ModelStateDictionary modelState)
        {
            StatusCode = (int)HttpStatusCode.UnprocessableEntity;
            ErrorMessage = "";
            foreach (var state in modelState)
            {
                ErrorMessage += $"Invalid format {state.Key}: {state.Value.Errors[0].ErrorMessage} ";
            }
        }
    }
}