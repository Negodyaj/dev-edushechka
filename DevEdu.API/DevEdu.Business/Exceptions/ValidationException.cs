using System;

namespace DevEdu.Business.Exceptions
{
    public class ValidationException : Exception
    { 
        public string Field { get; set; }
        public ValidationException(string message) : base(message) { }
    }
}