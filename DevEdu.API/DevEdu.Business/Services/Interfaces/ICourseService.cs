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
        void UpdateCourse(int id, CourseDto courseDto);
        void AddTopicToCourse(int courseId, int topicId, CourseTopicDto dto);
        void DeleteTopicFromCourse(int courseId, int topicId);
        List<CourseTopicDto> SelectAllTopicsByCourseId(int courseId);
        int AddCourseMaterialReference(int courseId, int materialId);
        int RemoveCourseMaterialReference(int courseId, int materialId);
    }
}