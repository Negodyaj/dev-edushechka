using System.ComponentModel.DataAnnotations;

namespace DevEdu.API.Models.InputModels
{
    public class NotificationAddInputModel
    {
        [Required]
        public int UserId { get; set; }
        [Required]
        public string Text { get; set; }
    }
}