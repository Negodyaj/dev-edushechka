using System.ComponentModel.DataAnnotations;

namespace DevEdu.API.Models.InputModels
{
    public class StudentAnswerOnTaskInputModel
    {
        [Required]
        public string Answer { get; set; }
    }
}