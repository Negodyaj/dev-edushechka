using System.ComponentModel.DataAnnotations;

namespace DevEdu.API.Models.InputModels
{
    public class CommentUpdateInputModel
    {
        [Required]
        public string Text { get; set; }
    }
}