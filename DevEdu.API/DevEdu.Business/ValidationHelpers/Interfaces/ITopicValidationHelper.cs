using DevEdu.DAL.Models;
using System.Collections.Generic;

namespace DevEdu.Business.ValidationHelpers
{
    public interface ITopicValidationHelper
    {
        void CheckTopicExistence(int topicId);
        void CheckTopicsExistence(List<CourseTopicDto> topics);
    }
}