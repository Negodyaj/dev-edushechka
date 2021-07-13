using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace DevEdu.API.Models.OutputModels
{
    public class LessonInfoWithGroupsOutputModel : LessonInfoOutputModel
    {
        public List<LessonInfoOutputModel> Groups { get; set; }
        [DefaultValue(false)]
        public bool IsDeleted { get; set; }
    }
}
