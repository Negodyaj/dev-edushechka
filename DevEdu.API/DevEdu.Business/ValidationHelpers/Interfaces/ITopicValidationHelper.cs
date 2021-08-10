using DevEdu.DAL.Models;
using System.Collections.Generic;

namespace DevEdu.Business.ValidationHelpers
{
    public interface ITopicValidationHelper
    {
        TopicDto GetTopicByIdAndThrowIfNotFound(int topicId);
        void GetTopicByListDtoAndThrowIfNotFound(List<CourseTopicDto> topics);
        CourseTopicDto GetCourseTopicByIdAndThrowIfNotFound(int id);
        List<CourseTopicDto> GetCourseTopicBySeveralIdAndThrowIfNotFound(List<int> ids);
    }
}