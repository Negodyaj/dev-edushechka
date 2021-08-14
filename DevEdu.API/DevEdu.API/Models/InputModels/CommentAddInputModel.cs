using DevEdu.API.Common;
using System.ComponentModel.DataAnnotations;

namespace DevEdu.API.Models.InputModels
{
    public class CommentAddInputModel
    {
        [Required(ErrorMessage = ValidationMessage.CommentTextRequired)]
        public string Text { get; set; }
    }
}