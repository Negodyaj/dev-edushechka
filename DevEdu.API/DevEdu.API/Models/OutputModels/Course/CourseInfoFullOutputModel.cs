using System.Collections.Generic;

namespace DevEdu.API.Models
{
    public class CourseInfoFullOutputModel : CourseInfoShortOutputModel
    {
        public List<MaterialOutputModel> Materials { get; set; }
        public List<TaskInfoOutputModel> Tasks { get; set; }
        public List<GroupOutputModel> Groups { get; set; }
    }
}