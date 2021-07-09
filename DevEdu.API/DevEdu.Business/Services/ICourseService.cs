using DevEdu.DAL.Models;
using System.Collections.Generic;

namespace DevEdu.Business.Services
{
    public interface ICourseService
    {
        void AddTopicToCourse(int courseId, int topicId, CourseTopicDto dto);
        void DeleteTopicFromCourse(int courseId, int topicId);
        List<CourseTopicDto> SelectAllTopicByCourseId(int courseId);
    }
}