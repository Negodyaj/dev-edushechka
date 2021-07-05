using System.ComponentModel.DataAnnotations;

namespace DevEdu.API.Models.InputModels
{
    public class StudentLessonUpdateFeedbackInputModel
    {
        [Required]
        public string Feedback { get; set; }
    }
}
