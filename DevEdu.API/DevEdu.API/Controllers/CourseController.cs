﻿using System.Collections.Generic;
using System.ComponentModel;
using AutoMapper;
using DevEdu.API.Models.InputModels;
using DevEdu.DAL.Models;
using Microsoft.AspNetCore.Mvc;
using DevEdu.API.Models.OutputModels;
using DevEdu.Business.Services;
using DevEdu.DAL.Repositories;
using Microsoft.AspNetCore.Http;
using DevEdu.API.Common;
using DevEdu.DAL.Enums;
using Microsoft.AspNetCore.Authorization;

namespace DevEdu.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class CourseController : Controller
    {
        private readonly IMapper _mapper;
        private readonly ICourseRepository _courseRepository;
        private readonly ICourseService _courseService;

        public CourseController(
            IMapper mapper,
            ICourseRepository courseRepository,
            ICourseService courseService)
        {
            _mapper = mapper;
            _courseRepository = courseRepository;
            _courseService = courseService;
            _mapper = mapper;
        }

        [HttpGet("{id}/simple")]
        [Description("Get course by id with groups")]
        [ProducesResponseType(typeof(CourseInfoFullOutputModel), StatusCodes.Status200OK)]
        public CourseInfoFullOutputModel GetCourseSimple(int id)
        {
            var course = _courseService.GetCourse(id);
            return _mapper.Map<CourseInfoFullOutputModel>(course);
        }

        [HttpGet("{id}/full")]
        [Description("Get course by id full")]
        [ProducesResponseType(typeof(CourseInfoFullOutputModel), StatusCodes.Status200OK)]
        public CourseInfoFullOutputModel GetCourseFull(int id)
        {
            var course = _courseService.GetFullCourseInfo(id);
            return _mapper.Map<CourseInfoFullOutputModel>(course);
        }

        [HttpGet]
        [Description("Get all courses")]
        [ProducesResponseType(typeof(CourseInfoFullOutputModel), StatusCodes.Status200OK)]
        public List<CourseInfoFullOutputModel> GetAllCoursesWithGrops()
        {
            var courses = _courseService.GetCourses();
            return _mapper.Map<List<CourseInfoFullOutputModel>>(courses);
        }


        [HttpPost]
        [Description("Create new course")]
        [ProducesResponseType(typeof(CourseInfoFullOutputModel), StatusCodes.Status201Created)]
        public CourseInfoFullOutputModel AddCourse([FromBody] CourseInputModel model)
        {
            var dto = _mapper.Map<CourseDto>(model);
            int id = _courseService.AddCourse(dto);
            return GetCourseSimple(id);
        }

        [HttpDelete("{id}")]
        [Description("Delete course by id")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public void DeleteCourse(int id)
        {
            _courseService.DeleteCourse(id);
        }

        [HttpPut("{id}")]
        [Description("Update course by Id")]
        [ProducesResponseType(typeof(CourseInfoFullOutputModel), StatusCodes.Status200OK)]
        public CourseInfoFullOutputModel UpdateCourse(int id, [FromBody] CourseInputModel model)
        {
            var dto = _mapper.Map<CourseDto>(model);
            _courseService.UpdateCourse(id, dto);
            return GetCourseSimple(id);
        }

        //  api/course/{CourseId}/Material/{MaterialId}
        [HttpPost("{courseId}/material/{materialId}")]
        [Description("Add material to course")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        public void AddMaterialToCourse(int courseId, int materialId)
        {
            _courseService.AddCourseMaterialReference(courseId, materialId);
        }

        [HttpDelete("{courseId}/material/{materialId}")]
        [Description("Delete material from course")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        public void RemoveMaterialFromCourse(int courseId, int materialId)
        {
            _courseService.RemoveCourseMaterialReference(courseId, materialId);
        }

        [HttpPost("{courseId}/task/{taskId}")]
        [Description("Add task to course")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        public string AddTaskToCourse(int courseId, int taskId)
        {
            _courseService.AddTaskToCourse(courseId, taskId);
            return $"Course {courseId} add  Task Id:{taskId}";
        }

        [HttpDelete("{courseId}/task/{taskId}")]
        [Description("Delete task from course")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        public string RemoveTaskFromCourse(int courseId, int taskId)
        {
            _courseService.DeleteTaskFromCourse(courseId, taskId);
            return $"Course {courseId} remove  Task Id:{taskId}";
        }

        // api/course/{courseId}/topic/{topicId}
        [HttpPost("{courseId}/topic/{topicId}")]
        [AuthorizeRoles(Role.Manager, Role.Methodist)]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [Description("Add topic to course")]
        public string AddTopicToCourse(int courseId, int topicId, [FromBody] CourseTopicInputModel inputModel)
        {
            var dto = _mapper.Map<CourseTopicDto>(inputModel);

            _courseService.AddTopicToCourse(courseId, topicId, dto);
            return $"Topic Id:{topicId} added in course Id:{courseId} on {inputModel.Position} position";
        }

        [HttpPost("{courseId}/select-topics")]
        [AuthorizeRoles(Role.Manager, Role.Methodist)]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [Description("Add topics to course")]
        public string AddTopicsToCourse(int courseId, [FromBody] List<CourseTopicUpdateInputModel> inputModel)
        {
            var dto = _mapper.Map<List<CourseTopicDto>>(inputModel);

            _courseService.AddTopicsToCourse(courseId, dto);
            return "done";
        }

        // api/course/{courseId}/topic/{topicId}
        [HttpDelete("{courseId}/topic/{topicId}")]
        [AuthorizeRoles(Role.Manager, Role.Methodist)]
        [Description("Delete topic from course")]
        [ProducesResponseType(typeof(string), StatusCodes.Status204NoContent)]
        public string DeleteTopicFromCourse(int courseId, int topicId)
        {
            _courseService.DeleteTopicFromCourse(courseId, topicId);
            return $"Topic Id:{topicId} deleted from course Id:{courseId}";
        }

        [HttpGet("{courseId}/topics")]
        [Description("Get all topics by course id ")]
        [ProducesResponseType(typeof(List<CourseTopicOutputModel>), StatusCodes.Status200OK)]
        public List<CourseTopicOutputModel> SelectAllTopicsByCourseId(int courseId)
        {
            var list = _courseService.SelectAllTopicsByCourseId(courseId);

            return _mapper.Map<List<CourseTopicOutputModel>>(list);
        }

        // api/course/{courseId}/program
        [HttpPut("{courseId}/program")]
        [AuthorizeRoles(Role.Manager, Role.Methodist)]
        [Description("updates topics in the course")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        public string UpdateCourseTopicsByCourseId(int courseId, [FromBody] List<CourseTopicUpdateInputModel> topics)
        {
            var list = _mapper.Map<List<CourseTopicDto>>(topics);
            _courseService.UpdateCourseTopicsByCourseId(courseId, list);
            return "updated";
        }
    }
}