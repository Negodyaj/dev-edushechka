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
        public string Text { get; set; }
        public UserDto User { get; set; }
        public DateTime Date { get; set; }
        public Role Role { get; set; } 
    }
}
