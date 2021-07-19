using System.ComponentModel;

namespace DevEdu.API.Models.OutputModels
{
    public class CourseInfoWithGroupsOutputModel : CourseInfoOutputModel
    {
        //public List<GroupDto> Groups { get; set; }
        [DefaultValue(false)]
        public bool IsDeleted { get; set; }
    }
}