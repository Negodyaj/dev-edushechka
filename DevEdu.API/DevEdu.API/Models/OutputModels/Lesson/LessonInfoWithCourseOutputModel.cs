namespace DevEdu.API.Models.OutputModels
{
    public class LessonInfoWithCourseOutputModel : LessonInfoOutputModel
    {
        public CourseInfoShortOutputModel Course { get; set; }
    }
}
