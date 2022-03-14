using System.Collections.Generic;

namespace DevEdu.DAL.Models
{
    public class MaterialDto : BaseDto
    {
        public string Content { get; set; }
        public string Link { get; set; }
        public List<CourseDto> Courses { get; set; }
    }
}