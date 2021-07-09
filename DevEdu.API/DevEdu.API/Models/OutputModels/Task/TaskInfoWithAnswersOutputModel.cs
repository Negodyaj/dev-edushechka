using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using DevEdu.DAL.Models;

namespace DevEdu.API.Models.OutputModels
{
    public class TaskInfoWithAnswersOutputModel : TaskInfoOutputModel
    {
        public List<StudentAnswerOnTaskInfoOutputModel> Answers { get; set; }
        [DefaultValue(false)]
        public bool IsDeleted { get; set; }
    }
}
