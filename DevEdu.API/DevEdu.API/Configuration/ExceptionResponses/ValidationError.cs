using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DevEdu.API.Configuration.ExceptionResponses
{
    public class ValidationError
    {
        //public int StatusCode { get; set; }
        public string Field { get; set; }
        public string ErrorMessage { get; set; }
    }
}
