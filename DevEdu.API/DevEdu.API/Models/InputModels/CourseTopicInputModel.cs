using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DevEdu.API.Models.InputModels
{
    public class CourseTopicInputModel
    {
        public int Position { get; set; }
        public int CourseId { get; set; }
        public int TopicId { get; set; }
    }
}
