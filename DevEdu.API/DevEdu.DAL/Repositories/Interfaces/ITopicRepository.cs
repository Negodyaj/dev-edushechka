using DevEdu.DAL.Models;
using System.Collections.Generic;

namespace DevEdu.DAL.Repositories
{
    public interface ITopicRepository
    {
        int AddTopic(TopicDto topicDto);
        void DeleteTopic(int id);
        List<TopicDto> GetAllTopics();
        TopicDto GetTopic(int id);
        void UpdateTopic(int id, TopicDto topicDto);
        int AddTopicToCourse(CourseTopicDto dto);
        void AddTopicsToCourse(List<CourseTopicDto> dto);
        void DeleteTopicFromCourse(int courseId, int topicId);
    }
}