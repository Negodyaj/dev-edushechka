using System;

namespace DevEdu.DAL.Models
{
    public class CommentDto : BaseDto
    {
        public string Text { get; set; }
        public UserDto User { get; set; }
        public StudentHomeworkDto StudentHomework { get; set; }
        public DateTime Date { get; set; }
    }
}