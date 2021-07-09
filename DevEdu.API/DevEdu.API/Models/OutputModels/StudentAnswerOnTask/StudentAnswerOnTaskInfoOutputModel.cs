using System.ComponentModel.DataAnnotations;

namespace DevEdu.API.Models.OutputModels
{
    public class StudentAnswerOnTaskInfoOutputModel
    {
        public int StudentId { get; set; }
        public int StatusId { get; set; }
        public string Answer { get; set; }
    }
}