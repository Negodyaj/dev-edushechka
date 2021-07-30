using DevEdu.Business.Constants;
using DevEdu.Business.Exceptions;
using DevEdu.Business.ValidationHelpers;
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
        private readonly ITaskRepository _taskRepository;
        private readonly IMaterialRepository _materialRepository;

        private readonly ICourseValidationHelper _courseValidationHelper;
        private readonly IMaterialValidationHelper _materialValidationHelper;

        public CourseService(ITopicRepository topicRepository,
                             ICourseRepository courseRepository,
                             ITaskRepository taskRepository,
                             IMaterialRepository materialRepository,
                             ICourseValidationHelper courseValidationHelper,
                             IMaterialValidationHelper materialValidationHelper)
        {
            _topicRepository = topicRepository;
            _courseRepository = courseRepository;
            _taskRepository = taskRepository;
            _materialRepository = materialRepository;
            _courseValidationHelper = courseValidationHelper;
            _materialValidationHelper = materialValidationHelper;
        }

        public int AddCourse(CourseDto courseDto) => _courseRepository.AddCourse(courseDto);

        public void DeleteCourse(int id) => _courseRepository.GetCourse(id);

        public CourseDto GetCourse(int id) => _courseRepository.GetCourse(id);
        public CourseDto GetFullCourseInfo(int id) 
        {
            var course = GetCourse(id);
            course.Tasks = _taskRepository.GetTaskByCourseId(course.Id);
            course.Materials = _materialRepository.GetMaterialsByCourseId(course.Id);
            course.Topics = _topicRepository.GetTopicsByCourseId(course.Id);
            return course;

        }
        public List<CourseDto> GetCoursesForAdmin()
        {
            var courses = _courseRepository.GetCourses();
            return courses;
        }
        public List<CourseDto> GetCourses() => _courseRepository.GetCourses();

        public void UpdateCourse(int id, CourseDto courseDto)
        {
            courseDto.Id = id;
            _courseRepository.UpdateCourse(courseDto);
        }

        public void AddTopicToCourse(int courseId, int topicId,CourseTopicDto dto)
        {
            dto.Course = new CourseDto() { Id = courseId };
            dto.Topic = new TopicDto { Id = topicId };
            _topicRepository.AddTopicToCourse(dto);
        }

        public void AddTopicsToCourse(int courseId, List<CourseTopicDto> listDto)
        {
            foreach (var topic in listDto)
                topic.Course = new CourseDto() { Id = courseId };
            _topicRepository.AddTopicsToCourse(listDto);
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

        public void DeleteTaskFromCourse(int courseId, int taskId) =>
            _courseRepository.DeleteTaskFromCourse(courseId, taskId);

        public void AddTaskToCourse(int courseId, int taskId) =>
            _courseRepository.AddTaskToCourse(courseId, taskId);

        public int AddCourseMaterialReference(int courseId, int materialId)
        {
            CheckCourseAndMaterialExistences(courseId, materialId);
            return _courseRepository.AddCourseMaterialReference(courseId, materialId);
        }
        public int RemoveCourseMaterialReference(int courseId, int materialId)
        {
            CheckCourseAndMaterialExistences(courseId, materialId);
            return _courseRepository.RemoveCourseMaterialReference(courseId, materialId);
        }

        public void UpdateCourseTopicsByCourseId(int courseId, List<CourseTopicDto> topics)
        {
            if (topics == null || topics.Count == 0)
                return;

            CheckUniquenessPositions(topics);
            CheckUniquenessTopics(topics);
            var topicsInDatabase = _courseRepository.SelectAllTopicsByCourseId(courseId);
            if (
                topicsInDatabase != null &&
                topicsInDatabase.Count != 0 &&
                topics.Count != topicsInDatabase.Count
            )
            {
                DeleteAllTopicsByCourseId(courseId);
                AddTopicsToCourse(courseId, topics);
            }
            else if (topicsInDatabase == null || topicsInDatabase.Count == 0)
            {
                AddTopicsToCourse(courseId, topics);
            }
            else
            {
                foreach (var topic in topics)
                {
                    topic.Course = new CourseDto() { Id = courseId };
                }
                _courseRepository.UpdateCourseTopicsByCourseId(topics);
            }
        }
        public void DeleteAllTopicsByCourseId(int courseId) => _courseRepository.DeleteAllTopicsByCourseId(courseId);
        private void CheckUniquenessPositions(List<CourseTopicDto> topics)
        {
            if (topics.GroupBy(n => n.Position).Any(c => c.Count() > 1))
            {
                throw new ValidationException(ServiceMessages.SamePositionsInCourseTopics);
            }
        }
        private void CheckUniquenessTopics(List<CourseTopicDto> topics)
        {
            if (topics.GroupBy(n => n.Topic.Id).Any(c => c.Count() > 1))
            {
                throw new ValidationException(ServiceMessages.SameTopicsInCourseTopics);
            }
        }
        private void CheckCourseAndMaterialExistences(int courseId, int materialId)
        {
            _courseValidationHelper.CheckCourseExistence(courseId);
            _materialValidationHelper.CheckMaterialExistence(materialId);
        }
    }
}