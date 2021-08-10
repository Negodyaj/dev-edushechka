using AutoMapper;
using DevEdu.API.Common;
using DevEdu.API.Configuration;
using DevEdu.API.Models.InputModels;
using DevEdu.API.Models.OutputModels;
using DevEdu.Business.Services;
using DevEdu.DAL.Enums;
using DevEdu.DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using DevEdu.API.Extensions;
using Microsoft.AspNetCore.Mvc;
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
        private readonly ClaimsIdentity _claimsIdentity;

        public CourseController(
            IMapper mapper,
            ICourseService courseService)
        {
            _mapper = mapper;
            _courseService = courseService;
            _mapper = mapper;
            //_claimsIdentity = this.User.Identity as ClaimsIdentity;
        }

        [HttpGet("{id}/simple")]
        [Description("Get course by id with topics")]
        [Authorize]
        [ProducesResponseType(typeof(CourseInfoShortOutputModel), StatusCodes.Status200OK)]
        public CourseInfoShortOutputModel GetCourseSimple(int id)
        {
            var course = _courseService.GetCourse(id);
            return _mapper.Map<CourseInfoShortOutputModel>(course);
        }

        [HttpGet("{id}/full")]
        [Description("Get course by id full")]
        [AuthorizeRoles(Role.Manager, Role.Methodist)]
        [ProducesResponseType(typeof(CourseInfoFullOutputModel), StatusCodes.Status200OK)]

        public CourseInfoFullOutputModel GetCourseFull(int id)
        {
            var userToken = this.GetUserIdAndRoles();
            var course = _courseService.GetFullCourseInfo(id, userToken);
            return _mapper.Map<CourseInfoFullOutputModel>(course);
        }

        [HttpGet]
        [Description("Get all courses")]
        [Authorize]
        [ProducesResponseType(typeof(CourseInfoShortOutputModel), StatusCodes.Status200OK)]
        public List<CourseInfoShortOutputModel> GetAllCoursesWithGrops()
        {
            var courses = _courseService.GetCourses();
            return _mapper.Map<List<CourseInfoShortOutputModel>>(courses);
        }

        [HttpPost]
        [Description("Create new course")]
        [AuthorizeRoles(Role.Manager, Role.Teacher, Role.Methodist)]
        [ProducesResponseType(typeof(CourseInfoShortOutputModel), StatusCodes.Status201Created)]
        public CourseInfoShortOutputModel AddCourse([FromBody] CourseInputModel model)
        {
            var dto = _mapper.Map<CourseDto>(model);
            int id = _courseService.AddCourse(dto);
            return GetCourseSimple(id);
        }

        [HttpDelete("{id}")]
        [Description("Delete course by id")]
        [AuthorizeRoles(Role.Manager)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]

        public void DeleteCourse(int id)
        {
            _courseService.DeleteCourse(id);
        }

        [HttpPut("{id}")]
        [Description("Update course by Id")]
        [AuthorizeRoles(Role.Manager)]
        [ProducesResponseType(typeof(CourseInfoShortOutputModel), StatusCodes.Status200OK)]
        public CourseInfoShortOutputModel UpdateCourse(int id, [FromBody] CourseInputModel model)
        {
            var dto = _mapper.Map<CourseDto>(model);
            var updDto = _courseService.UpdateCourse(id, dto);
            return _mapper.Map<CourseInfoShortOutputModel>(updDto);
        }

        //  api/course/{CourseId}/Material/{MaterialId}
        [AuthorizeRoles(Role.Methodist)]
        [HttpPost("{courseId}/material/{materialId}")]
        [AuthorizeRoles(Role.Manager, Role.Methodist)]
        [Description("Add material to course")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status404NotFound)]
        public void AddCourseMaterialReference(int courseId, int materialId)
        {
            _courseService.AddCourseMaterialReference(courseId, materialId);
        }

        //  api/course/{CourseId}/Material/{MaterialId}
        [AuthorizeRoles(Role.Methodist)]
        [HttpDelete("{courseId}/material/{materialId}")]
        [Description("Remove material from course")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status404NotFound)]
        public void RemoveCourseMaterialReference(int courseId, int materialId)
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
        [Description("Add topics to course")]
        public List<CourseTopicOutputModel> AddTopicsToCourse(int courseId, [FromBody] List<CourseTopicUpdateInputModel> inputModel)
        {
            var dto = _mapper.Map<List<CourseTopicDto>>(inputModel);

            var id = _courseService.AddTopicsToCourse(courseId, dto);
            dto = _courseService.GetCourseTopicBuSevealId(id);
            return _mapper.Map<List<CourseTopicOutputModel>>(dto);
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
        [ProducesResponseType(typeof(List<CourseTopicOutputModel>), StatusCodes.Status200OK)]
        public List<CourseTopicOutputModel> UpdateCourseTopicsByCourseId(int courseId, [FromBody] List<CourseTopicUpdateInputModel> topics)
        {
            var list = _mapper.Map<List<CourseTopicDto>>(topics);
            var ids = _courseService.UpdateCourseTopicsByCourseId(courseId, list);
            list = _courseService.GetCourseTopicBuSevealId(ids);
            return _mapper.Map<List<CourseTopicOutputModel>>(list);
        }
    }
}