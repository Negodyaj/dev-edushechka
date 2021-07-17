namespace DevEdu.DAL.Models
{
    public class StudentAnswerOnTaskForTaskDto : BaseDto
    {
        public string Status { get; set; }
        public string Answer { get; set; }
        public UserDto Student { get; set; }
    }
}