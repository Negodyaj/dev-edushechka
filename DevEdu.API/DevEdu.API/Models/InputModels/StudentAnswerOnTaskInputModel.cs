using System.ComponentModel.DataAnnotations;
﻿using DevEdu.API.Common;

namespace DevEdu.API.Models.InputModels
{
    public class StudentAnswerOnTaskInputModel
    {
        [Required(ErrorMessage = ValidationMessage.StudentAnswerRequired)]
        public string Answer { get; set; }
    }
}