using System.Collections.Generic;

namespace DevEdu.API.Models.OutputModels
{
    public class CourseInfoFullOutputModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<GroupInfoOutputModel> Groups { get; set; }
        public List<TopicOutputModel> Topics { get; set; }
        public List<MaterialOutputModel> Materials { get; set; }
        public List<TaskOutputModel> Tasks { get; set; }
    }
}
