using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevEdu.DAL.Models
{
   public class NotificationDto : BaseDto
    {
        public int UserId { get; set; }
        public string Text { get; set; }
    }
}
