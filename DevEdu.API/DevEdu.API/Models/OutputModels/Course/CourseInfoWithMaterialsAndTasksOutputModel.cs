using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace DevEdu.API.Models.OutputModels
{
    public class CourseInfoWithMaterialsAndTasksOutputModel : CourseInfoOutputModel
    {
        //public List<MaterialDto> Materials { get; set; }
        //public List<TaskDto> Tasks { get; set; }
        [DefaultValue(false)]
        public bool IsDeleted { get; set; }
    }
}