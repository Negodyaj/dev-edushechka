using AutoMapper;
using DevEdu.API.Models.InputModels;
using DevEdu.API.Models.OutputModels;
using DevEdu.Business.Services;
using DevEdu.DAL.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.ComponentModel;

namespace DevEdu.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RaitingController: Controller
    {
        private IRaitingService _service;

        private readonly IMapper _mapper;

        public RaitingController(IMapper mapper, IRaitingService service)
        {
            _service = service;
            _mapper = mapper;
        }

        // api/raiting
        [HttpPost]
        [Description("Add StudentRaiting to database")]
        [ProducesResponseType(typeof(int), StatusCodes.Status201Created)]
        public int AddStudentRaiting([FromBody] StudentRaitingInputModel model)
        {
            var dto = _mapper.Map<StudentRaitingDto>(model);
            return _service.AddStudentRaiting(dto);
        }

        // api/raiting/1
        [HttpDelete("{id}")]
        [Description("Delete StudentRaiting from database")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public void DeleteStudentRaiting(int id) => _service.DeleteStudentRaiting(id);

        // api/raiting/1
        [HttpPut("{id}")]
        [Description("Update StudentRaiting in database and return updated StudentRaiting")]
        [ProducesResponseType(typeof(StudentRaitingOutputModel), StatusCodes.Status200OK)]
        public StudentRaitingOutputModel UpdateStudentRaiting(int id, [FromBody] StudentRaitingInputModel model)
        {
            var dto = _mapper.Map<StudentRaitingDto>(model);
            dto.Id = id;
            _service.UpdateStudentRaiting(dto);
            return GetStudentRaitingById(id);
        }

        // api/raiting
        [HttpGet]
        [Description("Get all StudentRaitings from database")]
        [ProducesResponseType(typeof(List<StudentRaitingOutputModel>), StatusCodes.Status200OK)]
        public List<StudentRaitingOutputModel> GetAllStudentRaitings()
        {
            var queryResult = _service.GetAllStudentRaitings();
            return _mapper.Map<List<StudentRaitingOutputModel>>(queryResult);
        }

        // api/raiting/1
        [HttpGet("{id}")]
        [Description("Get StudentRaiting from database by ID")]
        [ProducesResponseType(typeof(StudentRaitingOutputModel), StatusCodes.Status200OK)]
        public StudentRaitingOutputModel GetStudentRaitingById(int id)
        {
            var queryResult = _service.GetStudentRaitingById(id);
            return _mapper.Map<StudentRaitingOutputModel>(queryResult);
        }

        // api/raiting/user/1
        [HttpGet("user/{userid}")]
        [Description("Get StudentRaitings from database by UserID")]
        [ProducesResponseType(typeof(List<StudentRaitingOutputModel>), StatusCodes.Status200OK)]
        public List<StudentRaitingOutputModel> GetStudentRaitingByUserId(int id)
        {
            var queryResult = _service.GetStudentRaitingByUserId(id);
            return _mapper.Map<List<StudentRaitingOutputModel>>(queryResult);
        }
    }
}
