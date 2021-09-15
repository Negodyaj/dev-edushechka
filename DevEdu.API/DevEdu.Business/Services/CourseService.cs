﻿using DevEdu.Business.Constants;
using DevEdu.Business.Exceptions;
using DevEdu.Business.IdentityInfo;
using DevEdu.Business.ValidationHelpers;
using DevEdu.DAL.Models;
using DevEdu.DAL.Repositories;
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
        private readonly IGroupRepository _groupRepository;
        private readonly ICourseValidationHelper _courseValidationHelper;
        private readonly ITopicValidationHelper _topicValidationHelper;
        private readonly ITaskValidationHelper _taskValidationHelper;
        private readonly IMaterialValidationHelper _materialValidationHelper;

        public CourseService
        (
            ICourseRepository courseRepository,
            ITopicRepository topicRepository,
            ITaskRepository taskRepository,
            IMaterialRepository materialRepository,
            IGroupRepository groupRepository,
            ICourseValidationHelper courseValidationHelper,
            ITopicValidationHelper topicValidationHelper,
            IMaterialValidationHelper materialValidationHelper,
            ITaskValidationHelper taskValidationHelper
        )
        {
            _courseRepository = courseRepository;
            _topicRepository = topicRepository;
            _taskRepository = taskRepository;
            _materialRepository = materialRepository;
            _groupRepository = groupRepository;
            _courseValidationHelper = courseValidationHelper;
            _topicValidationHelper = topicValidationHelper;
            _materialValidationHelper = materialValidationHelper;
            _taskValidationHelper = taskValidationHelper;
        }

        public CourseDto AddCourse(CourseDto courseDto)
        {
            int addedCourseId = _courseRepository.AddCourse(courseDto);
            return _courseRepository.GetCourse(addedCourseId);
        }

        public void DeleteCourse(int id)
        {
            _courseValidationHelper.GetCourseByIdAndThrowIfNotFound(id);
            _courseRepository.DeleteCourse(id);
        }

        public CourseDto GetCourse(int id)
        {
            var course = _courseValidationHelper.GetCourseByIdAndThrowIfNotFound(id);
            return course;
        }

        public CourseDto GetFullCourseInfo(int id, UserIdentityInfo userToken)
        {
            var course = GetCourse(id);

            course.Tasks = _taskRepository.GetTasksByCourseId(course.Id);
            course.Materials = _materialRepository.GetMaterialsByCourseId(course.Id);
            course.Groups = _groupRepository.GetGroupsByCourseId(course.Id);

            return course;
        }

        public List<CourseDto> GetCourses() => _courseRepository.GetCourses();

        public CourseDto UpdateCourse(int id, CourseDto courseDto)
        {
            _courseValidationHelper.GetCourseByIdAndThrowIfNotFound(id);
            courseDto.Id = id;
            _courseRepository.UpdateCourse(courseDto);
            return _courseRepository.GetCourse(id);
        }

        public int AddTopicToCourse(int courseId, int topicId, CourseTopicDto dto)
        {
            CheckCourseAndTopicExistences(courseId, topicId);
            dto.Course = new CourseDto() { Id = courseId };
            dto.Topic = new TopicDto { Id = topicId };
            return _topicRepository.AddTopicToCourse(dto);
        }

        public List<int> AddTopicsToCourse(int courseId, List<CourseTopicDto> listDto)
        {
            _courseValidationHelper.GetCourseByIdAndThrowIfNotFound(courseId);
            _topicValidationHelper.GetTopicByListDtoAndThrowIfNotFound(listDto);
            foreach (var topic in listDto)
            {
                topic.Course = new CourseDto() { Id = courseId };
            }
            return _topicRepository.AddTopicsToCourse(listDto);
        }

        public void DeleteTopicFromCourse(int courseId, int topicId)
        {
            CheckCourseAndTopicExistences(courseId, topicId);
            _topicRepository.DeleteTopicFromCourse(courseId, topicId);
        }

        public List<CourseTopicDto> SelectAllTopicsByCourseId(int courseId)
        {
            _courseValidationHelper.GetCourseByIdAndThrowIfNotFound(courseId);
            var list = _courseRepository.SelectAllTopicsByCourseId(courseId);
            return list;
        }

        public void DeleteTaskFromCourse(int courseId, int taskId)
        {
            _courseValidationHelper.GetCourseByIdAndThrowIfNotFound(courseId);
            _taskValidationHelper.GetTaskByIdAndThrowIfNotFound(taskId);
            _courseRepository.DeleteTaskFromCourse(courseId, taskId);
        }


        public void AddTaskToCourse(int courseId, int taskId)
        {
            _courseValidationHelper.GetCourseByIdAndThrowIfNotFound(courseId);
            _courseRepository.AddTaskToCourse(courseId, taskId);
        }


        public int AddCourseMaterialReference(int courseId, int materialId)
        {
            _courseValidationHelper.GetCourseByIdAndThrowIfNotFound(courseId);
            _materialValidationHelper.GetMaterialByIdAndThrowIfNotFound(materialId);
            return _courseRepository.AddCourseMaterialReference(courseId, materialId);
        }

        public void RemoveCourseMaterialReference(int courseId, int materialId)
        {
            _courseValidationHelper.GetCourseByIdAndThrowIfNotFound(courseId);
            _materialValidationHelper.GetMaterialByIdAndThrowIfNotFound(materialId);
            _courseRepository.RemoveCourseMaterialReference(courseId, materialId);
        }

        public List<int> UpdateCourseTopicsByCourseId(int courseId, List<CourseTopicDto> topics)
        {
            List<int> response;
            if (topics == null || topics.Count == 0)
                throw new EntityNotFoundException(ServiceMessages.EntityNotFoundMessage);
            _courseValidationHelper.GetCourseByIdAndThrowIfNotFound(courseId);
            _topicValidationHelper.GetTopicByListDtoAndThrowIfNotFound(topics);
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
                response = AddTopicsToCourse(courseId, topics);
            }
            else if (topicsInDatabase == null || topicsInDatabase.Count == 0)
            {
                response = AddTopicsToCourse(courseId, topics);
            }
            else
            {
                response = new List<int>();
                foreach (var topic in topics)
                {
                    topic.Course = new CourseDto() { Id = courseId };
                    response.Add(topic.Id);
                }
                _courseRepository.UpdateCourseTopicsByCourseId(topics);

            }
            return response;
        }
        public CourseTopicDto GetCourseTopicById(int id)
        {
            return _topicValidationHelper.GetCourseTopicByIdAndThrowIfNotFound(id);
        }

        public List<CourseTopicDto> GetCourseTopicBySeveralId(List<int> ids)
        {
            return _topicValidationHelper.GetCourseTopicBySeveralIdAndThrowIfNotFound(ids);
        }

        public void DeleteAllTopicsByCourseId(int courseId)
        {
            _courseValidationHelper.GetCourseByIdAndThrowIfNotFound(courseId);
            _courseRepository.DeleteAllTopicsByCourseId(courseId);
        }

        private static void CheckUniquenessPositions(List<CourseTopicDto> topics)
        {
            if (topics.GroupBy(n => n.Position).Any(c => c.Count() > 1))
            {
                throw new ValidationException(nameof(CourseTopicDto.Position), ServiceMessages.SamePositionsInCourseTopicsMessage);
            }
        }

        private static void CheckUniquenessTopics(List<CourseTopicDto> topics)
        {
            if (topics.GroupBy(n => n.Topic.Id).Any(c => c.Count() > 1))
            {
                throw new ValidationException(nameof(CourseTopicDto.Topic), ServiceMessages.SameTopicsInCourseTopicsMessage);
            }
        }

        private void CheckCourseAndTopicExistences(int courseId, int topicId)
        {
            _courseValidationHelper.GetCourseByIdAndThrowIfNotFound(courseId);
            _topicValidationHelper.GetTopicByIdAndThrowIfNotFound(topicId);

        }
    }
}