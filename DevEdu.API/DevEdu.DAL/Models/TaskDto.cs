using System.Collections.Generic;

namespace DevEdu.DAL.Models
{
    public class TaskDto : BaseDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Links { get; set; }
        public bool IsRequired { get; set; }
        public List<TagDto> Tags { get; set; }
        public List<StudentHomeworkDto> StudentAnswers { get; set; }
        public List<CourseDto> Courses { get; set; }
        public List<GroupDto> Groups { get; set; }
    }
}