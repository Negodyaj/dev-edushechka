using DevEdu.DAL.Models;

namespace DevEdu.Business.Services
{
    public interface ICourseService
    {
        void AddTopicToCourse(int courseId, int topicId, CourseTopicDto dto);
        void DeleteTopicFromCourse(int courseId, int topicId);
    }
}