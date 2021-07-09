using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DevEdu.API.Models.OutputModels
{
    public class CourseTopicOutputModel
    {
        public int Id { get; set; }
        public int Position { get; set; }
        public TopicOutputModel Topic { get; set; }
    }
}