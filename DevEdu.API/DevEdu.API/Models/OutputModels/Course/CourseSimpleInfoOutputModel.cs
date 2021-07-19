using System.Collections.Generic;
using DevEdu.API.Models.OutputModels;

namespace DevEdu.API.Models.OutputModels
{
    public class CourseSimpleInfoOutputModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<GroupInfoOutputModel> Groups { get; set; }
    }
}