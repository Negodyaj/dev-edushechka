using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DevEdu.API.Models.InputModels
{
    public class CourseTopicUpdateInputModel : CourseTopicInputModel
    {
        public int TopicId { get; set; }
    }
}
