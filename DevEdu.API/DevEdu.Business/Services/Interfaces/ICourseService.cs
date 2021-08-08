using DevEdu.DAL.Models;
using System.Collections.Generic;

namespace DevEdu.Business.Services
{
    public interface ICourseService
    {
        CourseDto GetCourse(int id);
        CourseDto GetFullCourseInfo(int id, UserDto userToken);
        int AddCourse(CourseDto courseDto);
        void DeleteCourse(int id);
        List<CourseDto> GetCourses();
        List<CourseDto> GetCoursesForAdmin();
        CourseDto UpdateCourse(int id, CourseDto courseDto);
        int AddTopicToCourse(int courseId, int topicId, CourseTopicDto dto);
        List<int> AddTopicsToCourse(int courseId, List<CourseTopicDto> listDto);
        List<int> UpdateCourseTopicsByCourseId(int courseId, List<CourseTopicDto> topics);
        void DeleteTopicFromCourse(int courseId, int topicId);
        List<CourseTopicDto> SelectAllTopicsByCourseId(int courseId);
        void DeleteTaskFromCourse(int courseId, int taskId);
        public void AddTaskToCourse(int courseId, int taskId);
        int AddCourseMaterialReference(int courseId, int materialId);
        void RemoveCourseMaterialReference(int courseId, int materialId);
        CourseTopicDto GetCourseTopicById(int id);
        List<CourseTopicDto> GetCourseTopicBuSevealId(List<int> ids);
    }
}