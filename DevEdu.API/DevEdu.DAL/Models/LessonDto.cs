using System;

namespace DevEdu.DAL.Models
{
    public class LessonDto : BaseDto
    {
        public DateTime Date { get; set; }
        public CommentDto TeacherComment { get; set; }
        public int TeacherId { get; set; }
    }
}