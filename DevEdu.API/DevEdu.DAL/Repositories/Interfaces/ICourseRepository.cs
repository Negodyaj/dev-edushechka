using System.Collections.Generic;
using DevEdu.DAL.Models;

namespace DevEdu.DAL.Repositories
{
    public interface ICourseRepository
    {
        int AddCourse(CourseDto courseDto);
        void DeleteCourse(int id);
        CourseDto GetCourse(int id);
        List<CourseDto> GetCourses();
        CourseDto UpdateCourse(CourseDto courseDto);
        void AddTaskToCourse(int courseId, int taskId);
        void DeleteTaskFromCourse(int courseId, int taskId);
        List<CourseTopicDto> SelectAllTopicsByCourseId(int courseId);
        void DeleteAllTopicsByCourseId(int courseId);
        void UpdateCourseTopicsByCourseId(List<CourseTopicDto> topics);
        List<CourseDto> GetCoursesToTaskByTaskId(int id);
        public List<CourseDto> GetCoursesByMaterialId(int id);
        int AddCourseMaterialReference(int courseId, int materialId);
        int RemoveCourseMaterialReference(int courseId, int materialId);
    }
}