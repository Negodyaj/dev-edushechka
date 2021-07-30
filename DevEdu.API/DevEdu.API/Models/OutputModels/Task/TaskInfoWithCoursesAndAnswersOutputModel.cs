using System.Collections.Generic;

namespace DevEdu.API.Models
{
    public class TaskInfoWithCoursesAndAnswersOutputModel : TaskInfoOutputModel
    {
        public List<CourseInfoShortOutputModel> Courses { get; set; }
        public List<StudentAnswerOnTaskInfoOutputModel> StudentAnswers { get; set; }
    }
}