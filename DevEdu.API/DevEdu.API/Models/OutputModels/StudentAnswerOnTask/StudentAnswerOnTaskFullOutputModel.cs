using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DevEdu.API.Models.OutputModels
{
    public class StudentAnswerOnTaskFullOutputModel : StudentAnswerOnTaskInfoOutputModel
    {
        public string CompletedDate { get; set; }
        public TaskStatus TaskStatus { get; set; }
    }
}
