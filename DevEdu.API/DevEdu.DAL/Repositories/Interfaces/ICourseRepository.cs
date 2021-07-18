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
        void UpdateCourse(CourseDto courseDto);
        void AddTaskToCourse(int courseId, int taskId);
        void DeleteTaskFromCourse(int courseId, int taskId);
        List<CourseTopicDto> SelectAllTopicsByCourseId(int courseId);
        List<CourseDto> GetCoursesToTaskByTaskId(int id);
        public List<CourseDto> GetCoursesByMaterialId(int id);
    }
}