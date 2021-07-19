using System.Collections.Generic;
using System.ComponentModel;
using AutoMapper;
using DevEdu.API.Models.InputModels;
using DevEdu.DAL.Models;
using Microsoft.AspNetCore.Mvc;
using DevEdu.API.Models.OutputModels;
using DevEdu.Business.Services;
using DevEdu.DAL.Repositories;
using Microsoft.AspNetCore.Http;

namespace DevEdu.API.Controllers
{
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

        [HttpGet("simple/{id}")]
        [Description("Get course by id with groups")]
        [ProducesResponseType(typeof(CommentInfoOutputModel), StatusCodes.Status200OK)]
        public CourseSimplePriceOutputModel GetCourseSimple(int id)
        {
            var course = _courseService.GetCourse(id);
            return _mapper.Map<CourseSimplePriceOutputModel>(course);
        }

        [HttpGet("full/{id}")]
        [Description("Get course by id full")]
        [ProducesResponseType(typeof(CommentInfoOutputModel), StatusCodes.Status200OK)]
        public CourseInfoFullOutputModel GetCourseFull(int id)
        {
            var course = _courseService.GetCourseForAdmin(id);
            return _mapper.Map<CourseInfoFullOutputModel>(course);
        }

        [HttpGet("simple")]
        [Description("Get all courses with groups")]
        [ProducesResponseType(typeof(CommentInfoOutputModel), StatusCodes.Status200OK)]
        public List<CourseSimpleInfoOutputModel> GetAllCoursesWithGrops()
        {
            var courses = _courseService.GetCourse();
            return _mapper.Map<List<CourseSimpleInfoOutputModel>>(courses);
        }

        [HttpGet("full")]
        [Description("Get all courses full")]
        [ProducesResponseType(typeof(CommentInfoOutputModel), StatusCodes.Status200OK)]
        public List<CourseInfoFullOutputModel> GetAllCoursesWithFull()
        {
            var courses = _courseService.GetCourseForAdmin();
            return _mapper.Map<List<CourseInfoFullOutputModel>>(courses);
        }

        [HttpPost]
        [Description("Create new course")]
        [ProducesResponseType(typeof(int), StatusCodes.Status201Created)]
        public int AddCourse([FromBody] CourseInputModel model)
        {
            var dto = _mapper.Map<CourseDto>(model);
            return _courseService.AddCourse(dto);
        }

        [HttpDelete("{id}")]
        [Description("Delete course by id")]
        [ProducesResponseType(typeof(CommentInfoOutputModel), StatusCodes.Status200OK)]
        public void DeleteCourse(int id)
        {
            _courseService.DeleteCourse(id);
        }

        [HttpPut("{id}")]
        [Description("Update course by Id")]
        [ProducesResponseType(typeof(CommentInfoOutputModel), StatusCodes.Status200OK)]
        public string UpdateCourse(int id, [FromBody] CourseInputModel model)
        {
            var dto = _mapper.Map<CourseDto>(model);
            _courseService.UpdateCourse(id, dto);
            return $"Course №{id} change name to {model.Name} and description to {model.Description}";
        }

        [HttpPost("topic/{topicId}/tag/{tagId}")]
        public string AddTagToTopic(int topicId, int tagId)
        {
            _courseService.AddTagToTopic(topicId, tagId);
            return $"add to topic with {topicId} Id tag with {tagId} Id";
        }

        [HttpDelete("topic/{topicId}/tag/{tagId}")]
        public string DeleteTagAtTopic(int topicId, int tagId)
        {
            _courseService.DeleteTagFromTopic(topicId, tagId);
            return $"deleted at topic with {topicId} Id tag with {tagId} Id";
        }

        [HttpPost("{courseId}/material/{materialId}")]
        [Description("Add material to course")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        public string AddMaterialToCourse(int courseId, int materialId)
        {
            return $"Course {courseId} add  Material Id {materialId}";
        }

        [HttpDelete("{courseId}/material/{materialId}")]
        [Description("Delete material from course")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        public string RemoveMaterialFromCourse(int courseId, int materialId)
        {
            return $"Course {courseId} remove  Material Id:{materialId}";
        }

        [HttpPost("{courseId}/task/{taskId}")]
        [Description("Add task to course")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        public string AddTaskToCourse(int courseId, int taskId)
        {
            _courseRepository.AddTaskToCourse(courseId, taskId);
            return $"Course {courseId} add  Task Id:{taskId}";
        }

        [HttpDelete("{courseId}/task/{taskId}")]
        [Description("Delete task from course")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        public string RemoveTaskFromCourse(int courseId, int taskId)
        {
            _courseRepository.DeleteTaskFromCourse(courseId, taskId);
            return $"Course {courseId} remove  Task Id:{taskId}";
        }

        [HttpPost("{courseId}/topic/{topicId}")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [Description("Add topic to course")]
        public string AddTopicToCourse(int courseId, int topicId, [FromBody] CourseTopicInputModel inputModel)
        {
            var dto = _mapper.Map<CourseTopicDto>(inputModel);

            _courseService.AddTopicToCourse(courseId, topicId, dto);
            return $"Topic Id:{topicId} added in course Id:{courseId} on {inputModel.Position} position";

        }
        // api/course/{courseId}/topic/{topicId}
        [HttpDelete("{courseId}/topic/{topicId}")]
        [Description("Delete topic from course")]
        [ProducesResponseType(typeof(string), StatusCodes.Status204NoContent)]
        public string DeleteTopicFromCourse(int courseId, int topicId)
        {
            _courseService.DeleteTopicFromCourse(courseId, topicId);
            return $"Topic Id:{topicId} deleted from course Id:{courseId}";
        }
        [HttpGet("{courseId}/topics")]
        [Description("Get all topics by course id ")]
        [ProducesResponseType(typeof(List<CourseTopicOutputModel>),StatusCodes.Status200OK)]
        public List<CourseTopicOutputModel> SelectAllTopicsByCourseId(int courseId)
        {
            var list = _courseService.SelectAllTopicsByCourseId(courseId);
            
            return _mapper.Map<List<CourseTopicOutputModel>>(list);
            
        }

    }
}