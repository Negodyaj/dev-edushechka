using System.Collections.Generic;
using DevEdu.API.Models.OutputModels.Group;

namespace DevEdu.API.Models.OutputModels
{
    public class CourseSimpleInfoOutputModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<GroupInfoOutputModel> CourseGroups { get; set; }
    }
}