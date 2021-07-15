using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace DevEdu.API.Models.OutputModels
{
    public class LessonInfoWithGroupsOutputModel : LessonInfoOutputModel
    {
        public List<LessonGroupOutputModel> Groups { get; set; }
    }
}
