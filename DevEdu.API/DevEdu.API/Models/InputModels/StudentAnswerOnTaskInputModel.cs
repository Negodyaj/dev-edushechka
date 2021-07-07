using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace DevEdu.API.Models.InputModels
{
    public class StudentAnswerOnTaskInputModel
    {
        [Required]
        public string Answer { get; set; }
    }
}
