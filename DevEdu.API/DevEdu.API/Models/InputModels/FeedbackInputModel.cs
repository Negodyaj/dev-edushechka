using System.ComponentModel.DataAnnotations;
using static DevEdu.API.Common.ValidationMessage;

namespace DevEdu.API.Models.InputModels
{
    public class FeedbackInputModel
    {
        [Required(ErrorMessage = FeedbackRequired)]
        public string Feedback { get; set; }
    }
}