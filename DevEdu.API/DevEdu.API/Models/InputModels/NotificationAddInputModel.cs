using System.ComponentModel.DataAnnotations;
using static DevEdu.API.Common.ValidationMessage;

namespace DevEdu.API.Models.InputModels
{
    public class NotificationAddInputModel
    {
        [Range(minimum: 1, maximum: int.MaxValue, ErrorMessage = WrongFormatId)]
        public int? UserId { get; set; }
        [Required(ErrorMessage = TextRequired)]
        public string Text { get; set; }
        [Range(minimum: 1, maximum: int.MaxValue, ErrorMessage = WrongFormatId)]
        public int? RoleId { get; set; }
        [Range(minimum: 1, maximum: int.MaxValue, ErrorMessage = WrongFormatId)]
        public int? GroupId { get; set; }
    }
}