using DevEdu.DAL.Enums;

namespace DevEdu.DAL.Models
{
    public class StudentLessonDto
    {
        public int Id { get; set; }
        public UserDto Student { get; set; }
        public LessonDto Lesson { get; set; }
        public string Feedback { get; set; }
        public IsPresent IsPresent { get; set; }
        public string AbsenceReason { get; set; }
    }
}