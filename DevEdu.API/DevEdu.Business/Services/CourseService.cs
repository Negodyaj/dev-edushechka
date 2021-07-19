using DevEdu.DAL.Models;
using DevEdu.DAL.Repositories;
using System.Collections.Generic;

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

        public List<CourseTopicDto> SelectAllTopicsByCourseId(int courseId)
        {
            var list = _courseRepository.SelectAllTopicsByCourseId(courseId);
            return list;
        }

        public int AddCourseMaterialReference(int courseId, int materialId) => _courseRepository.AddCourseMaterialReference(courseId, materialId);

        public int RemoveCourseMaterialReference(int courseId, int materialId) => _courseRepository.RemoveCourseMaterialReference(courseId, materialId);
    }
}