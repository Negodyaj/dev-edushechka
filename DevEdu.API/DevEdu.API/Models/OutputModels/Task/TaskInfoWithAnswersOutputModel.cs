using System.Collections.Generic;

namespace DevEdu.API.Models.OutputModels
{
    public class TaskInfoWithAnswersOutputModel : TaskInfoOutputModel
    {
        public List<StudentAnswerOnTaskOutputModel> StudentAnswers { get; set; }
    }
}