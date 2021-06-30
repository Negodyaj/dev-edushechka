using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DevEdu.API.Models.InputModels
{
    public class CommentAddtInputModel
    {
        [Required]
        public int UserId { get; set; }
        [Required]
        public string Text { get; set; }
    }
}