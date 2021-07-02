namespace DevEdu.DAL.Models
{
    public class StudentAnswerOnTaskDto
    {
        public int TaskId { get; set; }
        public int StudentId { get; set; }
        public string? Answer { get; set; }
    }
}
