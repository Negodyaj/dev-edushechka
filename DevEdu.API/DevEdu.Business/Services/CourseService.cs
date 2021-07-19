﻿using DevEdu.DAL.Models;
using DevEdu.DAL.Repositories;
using System.Collections.Generic;

namespace DevEdu.Business.Services
{
    public class CourseService : ICourseService
    {
        private readonly ICourseRepository _courseRepository;
        private readonly ITopicRepository _topicRepository;
        private readonly ITaskRepository _taskRepository;
        private readonly IMaterialRepository _materialRepository;
        public CourseService(ITopicRepository topicRepository,
                             ICourseRepository courseRepository,
                             ITaskRepository taskRepository,
                             IMaterialRepository materialRepository)
        {
            _topicRepository = topicRepository;
            _courseRepository = courseRepository;
            _taskRepository = taskRepository;
            _materialRepository = materialRepository;
        }

        public int AddCourse(CourseDto courseDto) => _courseRepository.AddCourse(courseDto);
        public void DeleteCourse(int id) => _courseRepository.DeleteCourse(id);
        public CourseDto GetCourse(int id) => _courseRepository.GetCourse(id);
        public CourseDto GetCourseForAdmin(int id) 
        {
            var course = GetCourse(id);
            return FillCourse(course, id);

        }
        public List<CourseDto> GetCourseForAdmin()
        {
            var courses = _courseRepository.GetCourse();
            foreach (var course in courses)
            {
                FillCourse(course, course.Id);
            }
            return courses;
        }
        public List<CourseDto> GetCourse() => _courseRepository.GetCourse();

        public void UpdateCourse(int id, CourseDto courseDto)
        {
            courseDto.Id = id;
            _courseRepository.UpdateCourse(courseDto);
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

        public List<CourseTopicDto> SelectAllTopicsByCourseId(int courseId)
        {
            var list = _courseRepository.SelectAllTopicsByCourseId(courseId);
            return list;
        }

        private CourseDto FillCourse(CourseDto course, int id)
        {
            course.Tasks = _taskRepository.GetTaskByCourseId(course.Id);
            course.Materials = _materialRepository.GetMaterialsByCourseId(course.Id);
            course.Topics = _topicRepository.GetTopicsByCourseId(course.Id);
            return course;
        }
    }
}