using System.Collections.Generic;

namespace DevEdu.DAL.Models
{
    public class MaterialDto : BaseDto
    {
        public string Content { get; set; }
        public List<CourseDto> Courses { get; set; }
        public List<GroupDto> Groups { get; set; }
    }
}