using System.ComponentModel.DataAnnotations;

namespace DevEdu.API.Models.InputModels
{
    public class FeedbackInputModel
    {
        [Required]
        public string Feedback { get; set; }
    }
}
