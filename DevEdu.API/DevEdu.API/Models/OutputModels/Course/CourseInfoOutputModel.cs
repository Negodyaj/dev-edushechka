using System.Collections.Generic;

namespace DevEdu.API.Models.OutputModels
{
    public class CourseInfoOutputModel : CourseInfoBaseOutputModel
    {
        public string Description { get; set; }
        public List<CourseTopicOutputModel> CourseTopics { get; set; }
    }
}