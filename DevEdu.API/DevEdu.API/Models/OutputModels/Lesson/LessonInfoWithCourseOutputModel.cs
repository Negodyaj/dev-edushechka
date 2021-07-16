using DevEdu.DAL.Models;

namespace DevEdu.API.Models.OutputModels
{
    public class LessonInfoWithCourseOutputModel : LessonInfoOutputModel
    {
        public CourseDto Course { get; set; }
    }
}
