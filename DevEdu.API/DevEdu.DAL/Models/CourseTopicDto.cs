using System.Collections.Generic;

namespace DevEdu.DAL.Models
{
    public class CourseTopicDto
    {
        public int Id { get; set; }
        public int Position { get; set; }
        public CourseDto Course { get; set; }
        public TopicDto Topic { get; set; }

    }
}