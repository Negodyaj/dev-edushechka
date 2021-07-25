using System.Collections.Generic;

namespace DevEdu.API.Models.OutputModels
{
    public class TaskInfoWithCoursesOutputModel : TaskInfoOutputModel
    {
        public List<CourseInfoShortOutputModel> Courses { get; set; }
    }
}