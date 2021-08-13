using DevEdu.Business.Exceptions;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Collections.Generic;

namespace DevEdu.API.Configuration
{
    public class ValidationExceptionResponse
    {
        public int Code { get; set; }
        public string Message { get; set; }
        public List<ValidationError> Errors { get; set; }

        public ValidationExceptionResponse(ValidationException exception)
        {
            Errors = new List<ValidationError>
            {
                new ValidationError {Code = 422, Field = exception.Field, Message = exception.Message}
            };
        }
        public ValidationExceptionResponse(ModelStateDictionary modelState)
        {
            Errors = new List<ValidationError>();
            foreach (var state in modelState)
            {
                Errors.Add(new ValidationError
                {
                    Code = 422,
                    Field = state.Key,
                    Message = state.Value.Errors[0].ErrorMessage
                });
            }
        }
    }
}