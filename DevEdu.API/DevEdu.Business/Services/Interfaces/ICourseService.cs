using DevEdu.DAL.Models;
using System.Collections.Generic;

namespace DevEdu.Business.Services
{
    public interface ICourseService
    {
        CourseDto GetCourse(int id);
        int AddCourse(CourseDto courseDto);
        void DeleteCourse(int id);
        List<CourseDto> GetCourses();
        List<CourseDto> GetCourseForAdmin();
        void UpdateCourse(int id, CourseDto courseDto);
        void AddTopicToCourse(int courseId, int topicId, CourseTopicDto dto);
        void DeleteTopicFromCourse(int courseId, int topicId);
        List<CourseTopicDto> SelectAllTopicsByCourseId(int courseId);
        void AddTagToTopic(int topicId, int tagId);
        void DeleteTagFromTopic(int topicId, int tagId);
    }
}