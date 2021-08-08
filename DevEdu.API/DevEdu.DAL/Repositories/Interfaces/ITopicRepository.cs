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
        int UpdateTopic(TopicDto topicDto);
        int AddTopicToCourse(CourseTopicDto dto);
        List<int> AddTopicsToCourse(List<CourseTopicDto> dto);
        void DeleteTopicFromCourse(int courseId, int topicId);
        List<TopicDto> GetTopicsByCourseId(int courseId);
        int AddTagToTopic(int topicId, int tagId);
        int DeleteTagFromTopic(int topicId, int tagId);
        CourseTopicDto GetCourseTopicById(int id);
        List<CourseTopicDto> GetCourseTopicBuSevealId(List<int> ids);
    }
}