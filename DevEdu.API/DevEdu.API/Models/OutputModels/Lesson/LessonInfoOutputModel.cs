using System.Collections.Generic;
using System.ComponentModel;

namespace DevEdu.API.Models.OutputModels
{
    public class LessonInfoOutputModel
    {
        public int Id { get; set; }
        public string Date { get; set; }
        public string TeacherComment { get; set; }
        public UserInfoShortOutputModel Teacher { get; set; }
        public string LinkToRecord { get; set; }
        public List<TopicOutputModel> Topics {get; set;}
        [DefaultValue(false)]
        public bool IsDeleted { get; set; }
    }
}