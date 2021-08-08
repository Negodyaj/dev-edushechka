using System.Collections.Generic;

namespace DevEdu.API.Models.OutputModels
{
    public class CourseInfoFullOutputModel : CourseInfoShortOutputModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<TopicOutputModel> Topics { get; set; }
        public List<MaterialInfoOutputModel> Materials { get; set; }
        public List<TaskInfoOutputModel> Tasks { get; set; }
        public List<GroupOutputModel> Groups { get; set; }
    }
}
