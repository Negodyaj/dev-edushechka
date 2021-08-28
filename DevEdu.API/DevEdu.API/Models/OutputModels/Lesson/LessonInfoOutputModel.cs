using System.Collections.Generic;
using System.ComponentModel;

namespace DevEdu.API.Models
{
    public class LessonInfoOutputModel : LessonShortInfoOutputModel
    {
        public UserInfoShortOutputModel Teacher { get; set; }
        public string LinkToRecord { get; set; }
        public List<TopicOutputModel> Topics { get; set; }
        [DefaultValue(false)]
        public bool IsDeleted { get; set; }
    }
}