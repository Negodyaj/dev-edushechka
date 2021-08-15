using System.ComponentModel.DataAnnotations;
using static DevEdu.API.Common.ValidationMessage;

namespace DevEdu.API.Models
{
    public class CommentUpdateInputModel
    {
        [Required(ErrorMessage = TextRequired)]
        public string Text { get; set; }
    }
}