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

        //  api/Course/5
        [HttpGet("{id}")]
        public CourseDto GetCourse(int id)
        {
            return _courseService.GetCourse(id);
        }

        //  api/Course
        [HttpGet]
        [Description("Get all courses with topics")]
        public List<CourseInfoOutputModel> GetAllCourses()
        {
            var courses = _courseRepository.GetCourses();
            return _mapper.Map<List<CourseInfoOutputModel>>(courses);
        }

        //  api/course
        [HttpPost]
        public int AddCourse([FromBody] CourseInputModel model)
        {
            var dto = _mapper.Map<CourseDto>(model);
            return _courseService.AddCourse(dto);
        }

        //  api/course/5
        [HttpDelete("{id}")]
        public void DeleteCourse(int id)
        {
            _courseService.DeleteCourse(id);
        }

        //  api/course/5
        [HttpPut("{id}")]
        public string UpdateCourse(int id, [FromBody] CourseInputModel model)
        {
            var dto = _mapper.Map<CourseDto>(model);
            _courseService.UpdateCourse(id, dto);
            return $"Course №{id} change name to {model.Name} and description to {model.Description}";
        }

        //  api/course/topic/{topicId}/tag/{tagId}
        [HttpPost("topic/{topicId}/tag/{tagId}")]
        public string AddTagToTopic(int topicId, int tagId)
        {
            _courseService.AddTagToTopic(topicId, tagId);
            return $"add to topic with {topicId} Id tag with {tagId} Id";
        }

        //  api/course/topic/{topicId}/tag/{tagId}
        [HttpDelete("topic/{topicId}/tag/{tagId}")]
        public string DeleteTagAtTopic(int topicId, int tagId)
        {
            _courseService.DeleteTagFromTopic(topicId, tagId);
            return $"deleted at topic with {topicId} Id tag with {tagId} Id";
        }

        //  api/course/{CourseId}/Material/{MaterialId}
        [HttpPost("{courseId}/material/{materialId}")]
        public string AddMaterialToCourse(int courseId, int materialId)
        {
            return $"Course {courseId} add  Material Id {materialId}";
        }

        //  api/course/{CourseId}/Material/{MaterialId}
        [HttpDelete("{courseId}/material/{materialId}")]
        public string RemoveMaterialFromCourse(int courseId, int materialId)
        {
            return $"Course {courseId} remove  Material Id:{materialId}";
        }

        //  api/course/{CourseId}/Task/{TaskId}
        [HttpPost("{courseId}/task/{taskId}")]
        public string AddTaskToCourse(int courseId, int taskId)
        {
            _courseRepository.AddTaskToCourse(courseId, taskId);
            return $"Course {courseId} add  Task Id:{taskId}";
        }

        //  api/course/{CourseId}/Task/{TaskId}
        [HttpDelete("{courseId}/task/{taskId}")]
        public string RemoveTaskFromCourse(int courseId, int taskId)
        {
            _courseRepository.DeleteTaskFromCourse(courseId, taskId);
            return $"Course {courseId} remove  Task Id:{taskId}";
        }

        // api/course/{courseId}/topic/{topicId}
        [HttpPost("{courseId}/topic/{topicId}")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [Description("Add topic to course")]
        public string AddTopicToCourse(int courseId, int topicId, [FromBody] CourseTopicInputModel inputModel)
        {
            var dto = _mapper.Map<CourseTopicDto>(inputModel);

            _courseService.AddTopicToCourse(courseId, topicId, dto);
            return $"Topic Id:{topicId} added in course Id:{courseId} on {inputModel.Position} position";
        }

        [HttpPost("{courseId}/select-topics")]
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

        // api/course/{courseId}/program
        [HttpPut("{courseId}/program")]
        [Description("updates topics in the course")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        public string UpdateCourseTopicsByCourseId(int courseId,  [FromBody] List<CourseTopicUpdateInputModel> topics)
        {
            var list = _mapper.Map<List<CourseTopicDto>>(topics);
            _courseService.UpdateCourseTopicsByCourseId(courseId, list);
            return "updated";
        }
    }
}