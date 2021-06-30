using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DevEdu.API.Models.InputModels
{
    public class LessonInputModel
    {
        [Required]
        public DateTime Date { get; set; }
        [Required]
        public string TeacherComment { get; set; }
        [Required]
        public int TeacherId { get; set; }
    }
}
