using DevEdu.Business.Exceptions;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;

namespace DevEdu.API.Configuration
{
    public class ValidationExceptionResponse
    {
        public const int CODE = 1001;
        public const string MESSAGE = "Validation Error";
        public List<ValidationError> ValidationErrors { get; set; }

        public ValidationExceptionResponse(Exception exception)
        {
            ValidationErrors = new List<ValidationError>();
            ValidationErrors.Add(new ValidationError
            {
                Code = 422,
                Message = exception.Message,
                Description = exception.Message
            });
        }
        public ValidationExceptionResponse(ModelStateDictionary modelState)
        {
            ValidationErrors = new List<ValidationError>();
            foreach (var state in modelState)
            {
                ValidationErrors.Add(new ValidationError
                {
                    Code = 422,
                    Message = state.Key,
                    Description = state.Value.Errors[0].ErrorMessage
                });
            }
        }
    }
}