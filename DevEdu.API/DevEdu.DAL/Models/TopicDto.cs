using System.Collections.Generic;

namespace DevEdu.DAL.Models
{
    public class TopicDto : BaseDto
    {
        public string Name { get; set; }
        public int Duration { get; set; }
        public List<CourseTopicDto> CourseTopics { get; set; }
    }
}
