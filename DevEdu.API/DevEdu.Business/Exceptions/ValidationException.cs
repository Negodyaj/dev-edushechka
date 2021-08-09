using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DevEdu.Business.Exceptions
{
    public class ValidationException : Exception
    {
        public int Code { get; set; }
        public string SuperMessage { get; set; }
        public List<ValidationError> ValidationErrors { get; set; }
        public ValidationException(string message) : base(message) { }
        
        public ValidationException(ModelStateDictionary modelState)
        {
            Code = 1001;
            SuperMessage = "Validation Failed";
            ValidationErrors = new List<ValidationError>();
            foreach (var state in modelState)
            {
                ValidationErrors.Add(new ValidationError
                {
                    Code=DateTime.Now.Millisecond,
                    Field = $"{state.Key}",
                    ErrorMessage = $"Invalid format {state.Value.Errors[0].ErrorMessage} "
                });
            }
        }
    }
}