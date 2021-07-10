using DevEdu.DAL.Models;
using System.Collections.Generic;

namespace DevEdu.DAL.Repositories
{
    public interface ICourseRepository
    {
        int AddCourse(CourseDto courseDto);
        int AddCourseMaterialReference(int courseId, int materialId);
        void AddTagToTopic(int topicId, int tagId);
        void DeleteCourse(int id);
        void DeleteTagFromTopic(int topicId, int tagId);
        CourseDto GetCourse(int id);
        List<CourseDto> GetCourses();
        int RemoveCourseMaterialReference(int courseId, int materialId);
        void UpdateCourse(CourseDto courseDto);
    }
}