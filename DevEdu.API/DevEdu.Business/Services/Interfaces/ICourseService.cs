using DevEdu.DAL.Models;
using System.Collections.Generic;

namespace DevEdu.Business.Services
{
    public interface ICourseService
    {
        CourseDto GetCourse(int id);
        CourseDto GetFullCourseInfo(int id);
        int AddCourse(CourseDto courseDto);
        void DeleteCourse(int id);
        List<CourseDto> GetCourses();
        List<CourseDto> GetCoursesForAdmin();
        void UpdateCourse(int id, CourseDto courseDto);
        void AddTopicToCourse(int courseId, int topicId, CourseTopicDto dto);
        void AddTopicsToCourse(int courseId, List<CourseTopicDto> listDto);
        void UpdateCourseTopicsByCourseId(int courseId, List<CourseTopicDto> topics);
        void DeleteTopicFromCourse(int courseId, int topicId);
        List<CourseTopicDto> SelectAllTopicsByCourseId(int courseId);
        void DeleteTaskFromCourse(int courseId, int taskId);
        public void AddTaskToCourse(int courseId, int taskId);
        int AddCourseMaterialReference(int courseId, int materialId);
        void RemoveCourseMaterialReference(int courseId, int materialId);
    }
}