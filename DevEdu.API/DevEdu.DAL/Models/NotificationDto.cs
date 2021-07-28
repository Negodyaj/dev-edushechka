using System;
using DevEdu.DAL.Enums;

namespace DevEdu.DAL.Models
    
{
    public class NotificationDto : BaseDto
    {
        public DateTime Date { get; set; }
        public string Text { get; set; }
        public Role? Role { get; set; }
        public UserDto User { get; set; }
        public GroupDto Group { get; set; }
       
       
    }
}