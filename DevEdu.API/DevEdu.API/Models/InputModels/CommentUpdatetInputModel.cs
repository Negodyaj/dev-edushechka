using System.ComponentModel.DataAnnotations;

namespace DevEdu.API.Models.InputModels
{
    public class CommentUpdatetInputModel
    {
        [Required]
        public string Text { get; set; }
    }
}