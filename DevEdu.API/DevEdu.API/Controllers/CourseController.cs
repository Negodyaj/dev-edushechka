﻿using System.Collections.Generic;
using System.ComponentModel;
using AutoMapper;
using DevEdu.API.Models.InputModels;
using DevEdu.API.Models.OutputModels;
using DevEdu.DAL.Models;
using DevEdu.DAL.Repositories;
using Microsoft.AspNetCore.Mvc;
using DevEdu.DAL.Repositories;
using AutoMapper;
using DevEdu.DAL.Models;

namespace DevEdu.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CourseController : Controller
    {
        private readonly IMapper _mapper;
        private readonly ICourseRepository _courseRepository;
        private readonly ITopicRepository _topicRepository;
        
        public CourseController(IMapper mapper, ICourseRepository courseRepository, ITopicRepository topicRepository )
        {
            _mapper = mapper;
            _courseRepository = courseRepository;
            _topicRepository = topicRepository;
            _mapper = mapper;
            _courseRepository = courseRepository;
        }

        //  api/Course/5
        [HttpGet("{id}")]
        public CourseDto GetCourse(int id)
        {
            return _courseRepository.GetCourse(id);
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
            return _courseRepository.AddCourse(dto);
        }

        //  api/course/5
        [HttpDelete("{id}")]
        public void DeleteCourse(int id)
        {
            _courseRepository.DeleteCourse(id);
        }

        //  api/course/5
        [HttpPut("{id}")]
        public string UpdateCourse(int id, [FromBody] CourseInputModel model)
        {
            var dto = _mapper.Map<CourseDto>(model);
            dto.Id = id;
            _courseRepository.UpdateCourse(dto);
            return $"Course №{id} change name to {model.Name} and description to {model.Description}";
        }

        //  api/course/topic/{topicId}/tag/{tagId}
        [HttpPost("topic/{topicId}/tag/{tagId}")]
        public string AddTagToTopic(int topicId, int tagId)
        {
            _courseRepository.AddTagToTopic(topicId, tagId);
            return $"add to topic with {topicId} Id tag with {tagId} Id";
        }

        //  api/course/topic/{topicId}/tag/{tagId}
        [HttpDelete("topic/{topicId}/tag/{tagId}")]
        public string DeleteTagAtTopic(int topicId, int tagId)
        {
            _courseRepository.DeleteTagFromTopic(topicId, tagId);
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
            return $"Course {courseId} add  Task Id:{taskId}";
        }

        //  api/course/{CourseId}/Task/{TaskId}
        [HttpDelete("{courseId}/task/{taskId}")]
        public string RemoveTaskFromCourse(int courseId, int taskId)
        {
            return $"Course {courseId} remove  Task Id:{taskId}";
        }
        // api/course/{courseId}/topic/{topicId}
        [HttpPost("{courseId}/topic/{topicId}")]
        public string AddTopicToCourse(int courseId, int topicId, [FromBody] CourseTopicInputModel inputModel)
        {
            var dto = _mapper.Map<CourseTopicDto>(inputModel);
            dto.Course = new CourseDto { Id = courseId };
            dto.Topic = new TopicDto { Id = topicId };

            _topicRepository.AddTopicToCourse(dto);
            return $"Topic Id:{topicId} added in course Id:{courseId} on {inputModel.Position} position";

        }
        // api/course/{courseId}/topic/{topicId}
        [HttpDelete("{courseId}/topic/{topicId}")]
        public string DeleteTopicFromCourse(int courseId, int topicId)
        {
            _topicRepository.DeleteTopicFromCourse(courseId, topicId);
            return $"Topic Id:{topicId} deleted from course Id:{courseId}";
        }
    }
}