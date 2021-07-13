using System;
using System.Collections.Generic;

namespace DevEdu.API.Models.OutputModels
{
    public class LessonInfoOutputModel
    {
        public DateTime Date { get; set; }
        public String TeacherComment { get; set; }
        public UserInfoOutputModel Teacher { get; set; }
    }
}
