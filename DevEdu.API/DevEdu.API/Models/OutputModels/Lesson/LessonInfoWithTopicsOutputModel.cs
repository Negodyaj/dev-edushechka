using System.Collections.Generic;
using System.ComponentModel;

namespace DevEdu.API.Models.OutputModels
{
    public class LessonInfoWithTopicsOutputModel : LessonInfoOutputModel
    {
        public List<TopicOutputModel> Topics { get; set; }
        [DefaultValue(false)]
        public bool IsDeleted { get; set; }
    }
}
