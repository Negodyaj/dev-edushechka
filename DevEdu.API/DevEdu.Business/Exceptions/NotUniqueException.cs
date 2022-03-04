using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevEdu.Business.Exceptions
{
    public class NotUniqueException : Exception
    {
        public string Field { get; set; }

        public NotUniqueException(string field) : base($"Field {field} must be unique. Provieded value already exist in DB.")
        {
            Field = field;
        }
    }
}
