using DevEdu.DAL.Enums;

namespace DevEdu.API.Models
{
    public class StudentHomeworkOutputModel
    {
        public int Id { get; set; }
        public string Answer { get; set; }
        public StudentHomeworkStatus StudentHomeworkStatus { get; set; }
        public string CompletedDate { get; set; }
        public UserInfoShortOutputModel User { get; set; }
        public int Rating { get; set; } 
        public bool IsDeleted { get; set; }
    }
}