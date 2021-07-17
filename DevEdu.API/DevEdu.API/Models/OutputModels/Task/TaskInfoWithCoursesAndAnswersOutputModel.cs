using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using DevEdu.DAL.Models;

namespace DevEdu.API.Models.OutputModels
{
    public class TaskInfoWithCoursesAndAnswersOutputModel : TaskInfoOutputModel
    {
        public List<CourseInfoShortOutputModel> Courses { get; set; }
        public List<StudentAnswerOnTaskInfoOutputModel> StudentAnswers { get; set; }
    }
}
