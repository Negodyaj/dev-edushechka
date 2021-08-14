using System;

namespace DevEdu.DAL.Models
{
    public class CommentDto : BaseDto
    {
        public string Text { get; set; }
        public UserDto User { get; set; }
        public LessonDto Lesson { get; set; }
        public StudentHomeworkDto StudentAnswer { get; set; }
        public DateTime Date { get; set; }
    }
}