using DevEdu.API.Configuration;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;

namespace DevEdu.Business.Exceptions
{
    public class ValidationExceptionExtensionResponse
    {
        public int Code { get; set; }
        public string Message { get; set; }
        public List<ValidationExceptionResponse> ValidationErrors { get; set; }

        public ValidationExceptionExtensionResponse(Exception exception)
        {
            ValidationErrors = new List<ValidationExceptionResponse>();
                ValidationErrors.Add(new ValidationExceptionResponse
                {
                    Code = 422,
                    Message = exception.Message,
                    Description = exception.Message
                });
        }
        public ValidationExceptionExtensionResponse(ModelStateDictionary modelState)
        {
            Code = 1001;
            Message = "Validation Failed";
            ValidationErrors = new List<ValidationExceptionResponse>();
            foreach (var state in modelState)
            {
                ValidationErrors.Add(new ValidationExceptionResponse
                {
                    Code = 422,
                    Message = state.Key,
                    Description = state.Value.Errors[0].ErrorMessage
                });
            }
        }
    }
}