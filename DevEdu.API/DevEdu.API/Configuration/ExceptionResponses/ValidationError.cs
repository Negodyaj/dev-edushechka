using DevEdu.API.Configuration;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;

namespace DevEdu.Business.Exceptions
{
    public class ValidationError
    {
        public int Code { get; set; }
        public string Field { get; set; }
        public string Message { get; set; }

    }
}