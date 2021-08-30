using DevEdu.API.Common;
using System.ComponentModel.DataAnnotations;

namespace DevEdu.API.Models
{
    public class CommentInputModel
    {
        [Required(ErrorMessage = ValidationMessage.TextRequired)]
        public string Text { get; set; }
    }
}