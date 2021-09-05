using AutoMapper;
using DevEdu.API.Common;
using DevEdu.API.Configuration.ExceptionResponses;
using DevEdu.API.Extensions;
using DevEdu.API.Models;
using DevEdu.Business.Services;
using DevEdu.DAL.Enums;
using DevEdu.DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace DevEdu.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class CourseController : Controller
    {
        private readonly IMapper _mapper;
        private readonly ICourseService _courseService;

        public CourseController(
            IMapper mapper,
            ICourseService courseService)
        {
            _mapper = mapper;
            _courseService = courseService;
            _mapper = mapper;
        }

        [HttpGet("{id}/simple")]
        [Description("Get course by id with topics")]
        [ProducesResponseType(typeof(CourseInfoShortOutputModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status404NotFound)]
        public CourseInfoShortOutputModel GetCourseSimple(int id)
        {
            var course = _courseService.GetCourse(id);
            return _mapper.Map<CourseInfoShortOutputModel>(course);
        }

        [HttpGet("{id}/full")]
        [Description("Get course by id full")]
        [AuthorizeRoles(Role.Manager, Role.Methodist)]
        [ProducesResponseType(typeof(CourseInfoFullOutputModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status404NotFound)]
        public CourseInfoFullOutputModel GetCourseFull(int id)
        {
            var userToken = this.GetUserIdAndRoles();
            var course = _courseService.GetFullCourseInfo(id, userToken);
            return _mapper.Map<CourseInfoFullOutputModel>(course);
        }

        [HttpGet]
        [Description("Get all courses")]
        [ProducesResponseType(typeof(CourseInfoShortOutputModel), StatusCodes.Status200OK)]
        public List<CourseInfoShortOutputModel> GetAllCoursesWithGroups()
        {
            var courses = _courseService.GetCourses();
            return _mapper.Map<List<CourseInfoShortOutputModel>>(courses);
        }

        [HttpPost]
        [Description("Create new course")]
        [AuthorizeRoles(Role.Manager, Role.Teacher, Role.Methodist)]
        [ProducesResponseType(typeof(CourseInfoShortOutputModel), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ValidationExceptionResponse), StatusCodes.Status422UnprocessableEntity)]
        public ActionResult<CourseInfoShortOutputModel> AddCourse([FromBody] CourseInputModel model)
        {
            var dto = _mapper.Map<CourseDto>(model);
            var course = _courseService.AddCourse(dto);
            var output = _mapper.Map<CourseInfoShortOutputModel>(course);
            return Created(new Uri($"api/Course/{output.Id}/full", UriKind.Relative), output);
        }

        [HttpDelete("{id}")]
        [Description("Delete course by id")]
        [AuthorizeRoles(Role.Manager)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status404NotFound)]
        public ActionResult DeleteCourse(int id)
        {
            _courseService.DeleteCourse(id);
            return NoContent();
        }

        [HttpPut("{id}")]
        [Description("Update course by Id")]
        [AuthorizeRoles(Role.Manager)]
        [ProducesResponseType(typeof(CourseInfoShortOutputModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ValidationExceptionResponse), StatusCodes.Status422UnprocessableEntity)]
        public CourseInfoShortOutputModel UpdateCourse(int id, [FromBody] CourseInputModel model)
        {
            var dto = _mapper.Map<CourseDto>(model);
            var updDto = _courseService.UpdateCourse(id, dto);
            return _mapper.Map<CourseInfoShortOutputModel>(updDto);
        }

        //  api/course/{courseId}/material/{materialId}
        [AuthorizeRoles(Role.Methodist)]
        [HttpPost("{courseId}/material/{materialId}")]
        [Description("Add material to course")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status404NotFound)]
        public ActionResult AddCourseMaterialReference(int courseId, int materialId)
        {
            _courseService.AddCourseMaterialReference(courseId, materialId);
            return NoContent();
        }

        //  api/course/{courseId}/material/{materialId}
        [AuthorizeRoles(Role.Methodist)]
        [HttpDelete("{courseId}/material/{materialId}")]
        [Description("Remove material from course")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status404NotFound)]
        public ActionResult RemoveCourseMaterialReference(int courseId, int materialId)
        {
            _courseService.RemoveCourseMaterialReference(courseId, materialId);
            return NoContent();
        }

        [HttpPost("{courseId}/task/{taskId}")]
        [Description("Add task to course")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status404NotFound)]
        public string AddTaskToCourse(int courseId, int taskId)
        {
            _courseService.AddTaskToCourse(courseId, taskId);
            return $"Task Id:{taskId} was added to the Course:{courseId}";
        }

        [HttpDelete("{courseId}/task/{taskId}")]
        [Description("Delete task from course")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status404NotFound)]
        public string RemoveTaskFromCourse(int courseId, int taskId)
        {
            _courseService.DeleteTaskFromCourse(courseId, taskId);
            return $"Task Id:{taskId} was deleted from Course:{courseId}";
        }

        // api/course/{courseId}/topic/{topicId}
        [HttpPost("{courseId}/topic/{topicId}")]
        [AuthorizeRoles(Role.Manager, Role.Methodist)]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ValidationExceptionResponse), StatusCodes.Status422UnprocessableEntity)]
        [Description("Add topic to course")]
        public CourseTopicOutputModel AddTopicToCourse(int courseId, int topicId, [FromBody] CourseTopicInputModel inputModel)
        {
            var dto = _mapper.Map<CourseTopicDto>(inputModel);
            var id = _courseService.AddTopicToCourse(courseId, topicId, dto);
            dto = _courseService.GetCourseTopicById(id);
            return _mapper.Map<CourseTopicOutputModel>(dto);
        }

        [HttpPost("{courseId}/add-topics")]
        [AuthorizeRoles(Role.Manager, Role.Methodist)]
        [ProducesResponseType(typeof(List<CourseTopicOutputModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ValidationExceptionResponse), StatusCodes.Status422UnprocessableEntity)]
        [Description("Add topics to course")]
        public List<CourseTopicOutputModel> AddTopicsToCourse(int courseId, [FromBody] List<CourseTopicUpdateInputModel> inputModel)
        {
            var dto = _mapper.Map<List<CourseTopicDto>>(inputModel);
            var id = _courseService.AddTopicsToCourse(courseId, dto);
            dto = _courseService.GetCourseTopicBySeveralId(id);
            return _mapper.Map<List<CourseTopicOutputModel>>(dto);
        }

        // api/course/{courseId}/topic/{topicId}
        [HttpDelete("{courseId}/topic/{topicId}")]
        [AuthorizeRoles(Role.Manager, Role.Methodist)]
        [Description("Delete topic from course")]
        [ProducesResponseType(typeof(string), StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status404NotFound)]
        public ActionResult DeleteTopicFromCourse(int courseId, int topicId)
        {
            _courseService.DeleteTopicFromCourse(courseId, topicId);
            return NoContent();
        }

        [HttpGet("{courseId}/topics")]
        [Description("Get all topics by course id")]
        [ProducesResponseType(typeof(List<CourseTopicOutputModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status404NotFound)]
        public List<CourseTopicOutputModel> SelectAllTopicsByCourseId(int courseId)
        {
            var list = _courseService.SelectAllTopicsByCourseId(courseId);
            return _mapper.Map<List<CourseTopicOutputModel>>(list);
        }

        // api/course/{courseId}/program
        [HttpPut("{courseId}/program")]
        [AuthorizeRoles(Role.Manager, Role.Methodist)]
        [Description("updates topics in the course")]
        [ProducesResponseType(typeof(List<CourseTopicOutputModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ValidationExceptionResponse), StatusCodes.Status422UnprocessableEntity)]
        public List<CourseTopicOutputModel> UpdateCourseTopicsByCourseId(int courseId, [FromBody] List<CourseTopicUpdateInputModel> topics)
        {
            var listDto = _mapper.Map<List<CourseTopicDto>>(topics);
            var listId = _courseService.UpdateCourseTopicsByCourseId(courseId, listDto);
            listDto = _courseService.GetCourseTopicBySeveralId(listId);
            return _mapper.Map<List<CourseTopicOutputModel>>(listDto);
        }
    }
}