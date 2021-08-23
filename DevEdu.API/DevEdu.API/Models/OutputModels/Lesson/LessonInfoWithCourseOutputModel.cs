namespace DevEdu.API.Models
{
    public class LessonInfoWithCourseOutputModel : LessonInfoOutputModel
    {
        public CourseInfoShortOutputModel Course { get; set; }
    }
}