using System.Collections.Generic;

namespace DevEdu.API.Models.OutputModels
{
    public class CourseInfoFullOutputModel : CourseInfoShortOutputModel
    {
        public List<TopicOutputModel> Topics { get; set; }
        public List<MaterialInfoOutputModel> Materials { get; set; }
        public List<TaskInfoOutputModel> Tasks { get; set; }
    }
}
