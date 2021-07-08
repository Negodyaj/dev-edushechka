using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevEdu.DAL.Models
{
    public class PaymentDto:BaseDto
    {
        public DateTime Data { get; set; }
        public int Summ { get; set; }
        public int UserId { get; set; }
        public bool IsPaid { get; set; }

    }
}
