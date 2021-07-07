using System.Collections.Generic;
using DevEdu.DAL.Models;

namespace DevEdu.Business.Servicies
{
    public interface ICourseService
    {
        CourseDto GetCourse(int id);
        int AddCourse(CourseDto courseDto);
        void DeleteCourse(int id);
        List<CourseDto> GetCourses();
        void UpdateCourse(int id, CourseDto courseDto);
        void AddTagToTopic(int topicId, int tagId);
        void DeleteTagFromTopic(int topicId, int tagId);
    }
}