using DevEdu.DAL.Enums;

namespace DevEdu.API.Models
{
    public class StudentLessonOutputModel
    {
        public int Id { get; set; }
        public UserInfoShortOutputModel Student { get; set; }
        public string Feedback { get; set; }
        public AttendanceType AttendanceType { get; set; }
        public string AbsenceReason { get; set; }
    }
}