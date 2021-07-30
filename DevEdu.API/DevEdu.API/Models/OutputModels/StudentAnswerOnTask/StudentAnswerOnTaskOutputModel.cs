using System.Threading.Tasks;

namespace DevEdu.API.Models
{
    public class StudentAnswerOnTaskOutputModel
    {
        public int Id { get; set; }
        public string Answer { get; set; }
        public TaskStatus TaskStatus { get; set; }
        public string CompletedDate { get; set; }
    }
}
