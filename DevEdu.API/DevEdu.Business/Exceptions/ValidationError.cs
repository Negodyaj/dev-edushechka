using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevEdu.Business.Exceptions
{
    public class ValidationError
    {
        public int Code { get; set; }
        public string Field { get; set; }
        public string ErrorMessage { get; set; }
    }
}