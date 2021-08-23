using System.Collections.Generic;

namespace DevEdu.API.Models
{
    public class TaskInfoWithAnswersOutputModel : TaskInfoOutputModel
    {
        public List<StudentHomeworkOutputModel> StudentAnswers { get; set; }
    }
}