using DevEdu.API.Common;
using System.ComponentModel.DataAnnotations;

namespace DevEdu.API.Models
{
    public class StudentAnswerOnTaskInputModel
    {
        [Required(ErrorMessage = ValidationMessage.StudentAnswerRequired)]
        public string Answer { get; set; }
    }
}