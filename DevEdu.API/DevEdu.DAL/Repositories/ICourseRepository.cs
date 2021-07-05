using DevEdu.DAL.Models;
using System.Collections.Generic;

namespace DevEdu.DAL.Repositories
{
    public interface ICourseRepository
    {
        int AddCourse(CourseDto courseDto);
        void DeleteCourse(int id);
        CourseDto GetCourse(int id);
        List<CourseDto> GetCourses();
        void UpdateCourse(CourseDto courseDto);
        void AddTagToTopic(int topicId, int tagId);
        void DeleteTagFromTopic(int topicId, int tagId);
    }
}