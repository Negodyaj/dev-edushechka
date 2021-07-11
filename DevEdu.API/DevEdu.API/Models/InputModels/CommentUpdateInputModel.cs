using System.ComponentModel.DataAnnotations;
using static DevEdu.API.Common.ValidationMessage;

namespace DevEdu.API.Models.InputModels
{
    public class CommentUpdateInputModel
    {
        [Required(ErrorMessage = TextRequired)]
        public string Text { get; set; }
    }
}