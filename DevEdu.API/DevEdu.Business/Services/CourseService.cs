using DevEdu.DAL.Models;
using DevEdu.DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;


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

        public int AddCourse(CourseDto courseDto) => _courseRepository.AddCourse(courseDto);
        public void DeleteCourse(int id) => _courseRepository.GetCourse(id);
        public CourseDto GetCourse(int id) => _courseRepository.GetCourse(id);
        public List<CourseDto> GetCourses() => _courseRepository.GetCourses();

        public void UpdateCourse(int id, CourseDto courseDto)
        {
            courseDto.Id = id;
            _courseRepository.UpdateCourse(courseDto);
        }

        public void AddTagToTopic(int topicId, int tagId) => _courseRepository.AddTagToTopic(topicId, tagId);

        public void DeleteTagFromTopic(int topicId, int tagId) => _courseRepository.DeleteTagFromTopic(topicId, tagId);
        public void AddTopicToCourse(int courseId, int topicId,CourseTopicDto dto)
        {
            dto.Course = SetIdToCourseDto(courseId, dto.Course);
            dto.Topic = new TopicDto { Id = topicId };
            _topicRepository.AddTopicToCourse(dto);
        }
        public void DeleteTopicFromCourse(int courseId, int topicId)
        {
            _topicRepository.DeleteTopicFromCourse(courseId, topicId);
        }

        public List<CourseTopicDto> SelectAllTopicsByCourseId(int courseId)
        {
            var list = _courseRepository.SelectAllTopicsByCourseId(courseId);
            return list;
        }
        public void UpdateCourseTopicsByCourseId(int courseId, List<CourseTopicDto> topics)
        {
            CheckUniquenessPositions(topics);
            var topicsInDatabase = _courseRepository.SelectAllTopicsByCourseId(courseId);
            if (((topics.Count > topicsInDatabase.Count) || (topics.Count < topicsInDatabase.Count)) && topicsInDatabase.Count!=0)
            {
                DeleteAllTopicsByCourseId(courseId);
                AddTopicsFromNewList(courseId, topics);
            }
            else if( topicsInDatabase.Count == 0)
            {
                AddTopicsFromNewList(courseId, topics);
            }
            else
            {
                foreach (var topic in topics)
                {
                    topic.Course = SetIdToCourseDto(courseId, topic.Course);
                }
                _courseRepository.UpdateCourseTopicsByCourseId(courseId, topics);
            }
        }
        public void DeleteAllTopicsByCourseId(int courseId)
        {
            _courseRepository.DeleteAllTopicsByCourseId(courseId);
        }
        private void AddTopicsFromNewList(int courseId, List<CourseTopicDto> topics)
        {
            foreach (var topic in topics)
            {
                AddTopicToCourse(courseId, topic.Topic.Id, topic);
            }
        }
        private void CheckUniquenessPositions(List<CourseTopicDto> topics)
        {
            if (topics.GroupBy(n => n.Position).Any(c => c.Count() > 1))
            {
                throw new Exception("the same positions of topics in the course");
            }
        }
        private CourseDto SetIdToCourseDto(int courseId, CourseDto dto)
        {
            return dto = new CourseDto() { Id = courseId };
        }
    }
}