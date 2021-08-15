using System.Collections.Generic;

namespace DevEdu.API.Models
{
    public class MaterialInfoFullOutputModel : MaterialInfoOutputModel
    {
        public List<CourseInfoBaseOutputModel> Courses { get; set; }
        public List<GroupInfoOutputModel> Groups { get; set; }
    }
}