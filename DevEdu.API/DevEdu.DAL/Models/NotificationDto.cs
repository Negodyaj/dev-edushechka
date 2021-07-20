using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevEdu.DAL.Enums;


namespace DevEdu.DAL.Models
{
    public class NotificationDto : BaseDto
    {
        public DateTime Date { get; set; }
        public string Text { get; set; }
        public Role? Role { get; set; }
        public UserDto User { get; set; }
       
         
    }
}
