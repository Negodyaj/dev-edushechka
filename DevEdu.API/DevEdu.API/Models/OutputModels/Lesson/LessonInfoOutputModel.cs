using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace DevEdu.API.Models.OutputModels
{
    public class LessonInfoOutputModel
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public String TeacherComment { get; set; }
        public UserInfoOutputModel Teacher { get; set; }
        public List<TopicOutputModel> Topics {get; set;}
        [DefaultValue(false)]
        public bool IsDeleted { get; set; }
    }
}
