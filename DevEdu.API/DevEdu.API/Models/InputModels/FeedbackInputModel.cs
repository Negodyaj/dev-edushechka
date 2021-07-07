using System.ComponentModel.DataAnnotations;

namespace DevEdu.API.Models.InputModels
{
    public class FeedbackInputModel
    {
        [Required(ErrorMessage = FeedbackRequired)]
        public string Feedback { get; set; }
    }
}
