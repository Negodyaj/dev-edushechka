using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DevEdu.API.Models.InputModels
{
    public class StudentLessonUpdateFeedbackInputModel
    {
        [Required]
        public string Feedback { get; set; }
    }
}
