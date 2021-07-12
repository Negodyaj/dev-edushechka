namespace DevEdu.DAL.Models
{
    public class StudentAnswerOnTaskForTaskDto : BaseDto
    {
        public string StudentFirstName { get; set; }
        public string StudentLastName { get; set; }
        public string Status { get; set; }
        public string Answer { get; set; }
    }
}