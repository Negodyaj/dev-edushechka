using System;
using System.ComponentModel.DataAnnotations;

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
