using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevEdu.DAL.Models
{
    public class TopicDto : BaseDto
    {
        public string Name { get; set; }
        public int Duration { get; set; }
        public List<CourseTopicDto> CourseTopics { get; set; }
    }
}
