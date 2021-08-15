using System.ComponentModel.DataAnnotations;

namespace DevEdu.API.Models
{
    public class NotificationUpdateInputModel
    {
        [Required]
        public string Text { get; set; }
    }
}