using DevEdu.DAL.Models;

namespace DevEdu.DAL.Repositories
{
    public interface ITopicRepository
    {
        int AddTopicToCourse(CourseTopicDto dto);
        void DeleteTopicFromCourse(int courseId, int topicId);
    }
}