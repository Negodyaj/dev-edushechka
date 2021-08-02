using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Net;

namespace DevEdu.API.Configuration.ExceptionResponses
{
    public class ValidationExceptionResponse : Exception
    {
        public List<ValidationError> ValidationErrors { get; set; }

        public ValidationExceptionResponse(ModelStateDictionary modelState)
        {
            ValidationErrors = new List<ValidationError>();
            foreach (var state in modelState)
            {
                ValidationErrors.Add(new ValidationError
                {
                    Field = $"{state.Key}",
                    ErrorMessage = $"Invalid format {state.Value.Errors[0].ErrorMessage} "
                });
            }
        }
    }
}