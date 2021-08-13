using System.Collections.Generic;

namespace DevEdu.API.Models.OutputModels
{
    public class CourseInfoShortOutputModel: CourseInfoBaseOutputModel
    {
        public string Description { get; set; }
        public List<TopicOutputModel> Topics { get; set; }
    }
}