using System;
using System.ComponentModel.DataAnnotations;
using static DevEdu.API.Common.ValidationMessage;

namespace DevEdu.API.Models.InputModels
{
    public class LessonInputModel
    {
        [Required(ErrorMessage = DateRequired)]
        public DateTime Date { get; set; }
        [Required(ErrorMessage = TeacherCommentRequired)]
        public string TeacherComment { get; set; }
        [Required(ErrorMessage = TeacherIdRequired)]
        public int TeacherId { get; set; }
    }
}