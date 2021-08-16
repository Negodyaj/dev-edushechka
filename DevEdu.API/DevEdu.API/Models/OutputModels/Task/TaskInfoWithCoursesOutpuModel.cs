using System.Collections.Generic;

namespace DevEdu.API.Models
{
    public class TaskInfoWithCoursesOutputModel : TaskInfoOutputModel
    {
        public List<CourseInfoShortOutputModel> Courses { get; set; }
    }
}