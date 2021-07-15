using System.ComponentModel.DataAnnotations;
using static DevEdu.API.Common.ValidationMessage;

namespace DevEdu.API.Models.InputModels
{
    public class NotificationAddInputModel
    {
        [Required(ErrorMessage = UserIdRequired)]
        public int UserId { get; set; }
        [Required(ErrorMessage = TextRequired)]
        public string Text { get; set; }
        [Required(ErrorMessage = RoleIdRequired)]
        public int RoleId { get; set; }
    }
}