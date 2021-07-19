using System.Collections.Generic;
using DevEdu.DAL.Models;

namespace DevEdu.DAL.Repositories
{
    public interface ICourseRepository
    {
        int AddCourse(CourseDto courseDto);
        void DeleteCourse(int id);
        CourseDto GetCourse(int id);
        List<CourseDto> GetCourse();
        void UpdateCourse(CourseDto courseDto);
        void AddTagToTopic(int topicId, int tagId);
        void DeleteTagFromTopic(int topicId, int tagId);
        void AddTaskToCourse(int courseId, int taskId);
        void DeleteTaskFromCourse(int courseId, int taskId);
        List<CourseTopicDto> SelectAllTopicsByCourseId(int courseId);
    }
}