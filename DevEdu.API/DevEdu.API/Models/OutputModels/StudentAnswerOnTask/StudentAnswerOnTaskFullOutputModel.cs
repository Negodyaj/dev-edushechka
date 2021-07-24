using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DevEdu.API.Models.OutputModels
{
    public class StudentAnswerOnTaskFullOutputModel
    {
        public int Id { get; set; }
        public UserInfoShortOutputModel User { get; set; }
        public string Answer { get; set; }
        public TaskStatus TaskStatus { get; set; }
        public string CompletedDate { get; set; }
        
    }
}
