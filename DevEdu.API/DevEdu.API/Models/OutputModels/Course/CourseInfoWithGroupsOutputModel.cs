using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace DevEdu.API.Models.OutputModels
{
    public class CourseInfoWithGroupsOutputModel : CourseInfoOutputModel
    {
        //public List<GroupDto> Groups { get; set; }
        [DefaultValue(false)]
        public bool IsDeleted { get; set; }
    }
}