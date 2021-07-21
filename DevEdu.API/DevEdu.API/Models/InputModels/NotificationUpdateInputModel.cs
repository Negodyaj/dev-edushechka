using System.ComponentModel.DataAnnotations;

namespace DevEdu.API.Models.InputModels
{
    public class NotificationUpdateInputModel
    {
        [Required]
        public string Text { get; set; }
    }
}