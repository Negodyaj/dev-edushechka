using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DevEdu.API.Models.InputModels
{
    public class CommentInputModel
    {
        [Required]
        public int UserId { get; set; }
        public string Text { get; set; }
    }
}