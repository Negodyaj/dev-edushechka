using DevEdu.DAL.Enums;

namespace DevEdu.API.Models
{
    public class StudentLessonShortOutputModel
    {
        public int Id { get; set; }
        public string Feedback { get; set; }
        public AttendanceType AttendanceType { get; set; }
        public string AbsenceReason { get; set; }

    }
}