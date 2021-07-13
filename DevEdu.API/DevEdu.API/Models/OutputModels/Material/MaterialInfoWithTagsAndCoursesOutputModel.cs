using System.Collections.Generic;

namespace DevEdu.API.Models.OutputModels
{
    public class MaterialInfoWithTagsAndCoursesOutputModel : MaterialInfoWithTagsOutputModel
    {
        public List <CourseInfoBaseOutputModel> Courses { get; set; }
    }
}
