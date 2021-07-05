using System.Collections.Generic;
using AutoMapper;
using DevEdu.API.Models.InputModels;
using DevEdu.DAL.Models;
using DevEdu.DAL.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace DevEdu.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CourseController : Controller
    {
        private readonly IMapper _mapper;
        private readonly ICourseRepository _courseRepository;
        public CourseController(IMapper mapper, ICourseRepository courseRepository)
        {
            _courseRepository = courseRepository;
            _mapper = mapper;
        }

        //  api/Course/5
        [HttpGet("{id}")]
        public CourseDto GetCourse(int id)
        {
            return _courseRepository.GetCourse(id);
        }

        //  api/Course
        [HttpGet]
        public List<CourseDto> GetAllCourses()
        {
            return _courseRepository.GetCourses();
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

    }
}