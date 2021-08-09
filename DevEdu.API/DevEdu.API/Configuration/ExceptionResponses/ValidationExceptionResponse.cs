using DevEdu.Business.Exceptions;

namespace DevEdu.API.Configuration
{
    public class ValidationExceptionResponse
    {
        public int Code { get; set; }
        public string Message { get; set; }
        public string Description { get; set; }
        public ValidationException Errors { get; set; }
    }
}