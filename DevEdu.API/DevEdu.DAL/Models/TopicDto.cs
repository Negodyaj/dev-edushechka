using System.Collections.Generic;

namespace DevEdu.DAL.Models
{
    public class TopicDto : BaseDto
    {
        public string Name { get; set; }
        public int Duration { get; set; }
        public int Position { get; set; }
        public List<CourseTopicDto> CourseTopics { get; set; }
        public List<TagDto> Tags { get; set; }
    }
}