using System;

namespace DevEdu.Business.Exceptions
{
    public class ValidationException : Exception
    {
        public string Message { get; set; }
        public string Description { get; set; }
        public ValidationException(string message) : base(message) { }
        public ValidationException() { }
    }

}
