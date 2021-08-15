namespace DevEdu.API.Models
{
    public class MaterialInfoWithCoursesOutputModel : MaterialInfoOutputModel
    {
        public List<CourseInfoBaseOutputModel> Courses { get; set; }
    }
}
