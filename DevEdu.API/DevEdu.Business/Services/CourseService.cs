using DevEdu.DAL.Models;
using DevEdu.DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text;

namespace DevEdu.Business.Services
{
    public class CourseService : ICourseService
    {
        private readonly ICourseRepository _courseRepository;
        private readonly ITopicRepository _topicRepository;
        public CourseService(ITopicRepository topicRepository, ICourseRepository courseRepository)
        {
            _topicRepository = topicRepository;
            _courseRepository = courseRepository;
        }

        public void AddTagToTopic(int topicId, int tagId) => _courseRepository.AddTagToTopic(topicId, tagId);

        public void DeleteTagFromTopic(int topicId, int tagId) => _courseRepository.DeleteTagFromTopic(topicId, tagId);
        public void AddTopicToCourse(int courseId, int topicId,CourseTopicDto dto)
        {
            dto.Course = new CourseDto { Id = courseId };
            dto.Topic = new TopicDto { Id = topicId };
            _topicRepository.AddTopicToCourse(dto);
        }
        public void DeleteTopicFromCourse(int courseId, int topicId)
        {
            _topicRepository.DeleteTopicFromCourse(courseId, topicId);
        }

        public List<CourseTopicDto> SelectAllTopicByCourseId(int courseId)
        {
            var list = _courseRepository.SelectAllTopicsByCourseId(courseId);
            return list;
        }
    }
}