using DevEdu.API.Common;
using DevEdu.API.Models.OutputModels;
using DevEdu.DAL.Models;
using System.ComponentModel.DataAnnotations;


namespace DevEdu.API.Models.InputModels
{
    public class CommentAddForStudentAnswerInputModel
    {
        [Required(ErrorMessage = ValidationMessage.CommentUserIdRequired)]
        [Range(minimum: 1, maximum: int.MaxValue, ErrorMessage = ValidationMessage.IdRequired)]
        public int UserId { get; set; }

        public UserDto User { get; set; }

        [Required(ErrorMessage = ValidationMessage.CommentTextRequired)]
        public string Text { get; set; }
    }
}
