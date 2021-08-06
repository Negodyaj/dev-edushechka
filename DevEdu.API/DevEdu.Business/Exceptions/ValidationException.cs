using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;

namespace DevEdu.Business.Exceptions
{
    public class ValidationException : Exception
    {
        public List<ValidationError> ValidationErrors { get; set; }
        public ValidationException(string message) : base(message) { }
        
        public ValidationException(ModelStateDictionary modelState)
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
