using System;
using System.ComponentModel.DataAnnotations;
using static DevEdu.API.Common.ValidationMessage;

namespace DevEdu.API.Models.InputModels
{
    public class LessonUpdateInputModel
    {
        [Required(ErrorMessage = TeacherCommentRequired)]
        public string TeacherComment { get; set; }

        [Required(ErrorMessage = LinkToRecordIdRequired)]
        [Url]
        public string LinkToRecord { get; set; }

        [Required(ErrorMessage = DateRequired)]
        public string Date { get; set; }
    }
}
