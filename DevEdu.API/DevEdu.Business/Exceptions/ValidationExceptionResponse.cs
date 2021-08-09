using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;

namespace DevEdu.Business.Exceptions
{
    public class ValidationExceptionResponse
    {
        public int Code { get; set; }
        public string Message { get; set; }
        public List<ValidationError> ValidationErrors { get; set; }

        public ValidationExceptionResponse(ModelStateDictionary modelState)
        {
            Code = 1001;
            Message = "Validation Failed";
            ValidationErrors = new List<ValidationError>();
            foreach (var state in modelState)
            {
                ValidationErrors.Add(new ValidationError
                {
                    Code = DateTime.Now.Millisecond + DateTime.Now.Minute,
                    Field = $"{state.Key}",
                    ErrorMessage = $"Invalid format {state.Value.Errors[0].ErrorMessage} "
                });
            }
        }
    }
}