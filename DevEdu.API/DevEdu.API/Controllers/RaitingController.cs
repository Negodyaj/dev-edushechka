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

        // api/raiting/1/{periodNumber}/value/50
        [HttpPut("{id}/period/{periodNumber}/value/{value}")]
        [Description("Update StudentRaiting in database and return updated StudentRaiting")]
        [ProducesResponseType(typeof(StudentRaitingOutputModel), StatusCodes.Status200OK)]
        public StudentRaitingOutputModel UpdateStudentRaiting(int id, int value, int periodNumber)
        {
            var queryResult = _service.UpdateStudentRaiting(id, value, periodNumber);
            return _mapper.Map<StudentRaitingOutputModel>(queryResult);
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


        // api/raiting/by-group/1
        [HttpGet("by-group/{groupId}")]
        [Description("Get StudentRaiting from database by GroupID")]
        [ProducesResponseType(typeof(StudentRaitingOutputModel), StatusCodes.Status200OK)]
        public List<StudentRaitingOutputModel> GetStudentRaitingByGroupID(int groupId)
        {
            var queryResult = _service.GetStudentRaitingByGroupId(groupId);
            return _mapper.Map<List<StudentRaitingOutputModel>>(queryResult);
        }

        // api/raiting/by-user/1
        [HttpGet("by-user/{userid}")]
        [Description("Get StudentRaitings from database by UserID")]
        [ProducesResponseType(typeof(List<StudentRaitingOutputModel>), StatusCodes.Status200OK)]
        public List<StudentRaitingOutputModel> GetStudentRaitingByUserId(int userid)
        {
            var queryResult = _service.GetStudentRaitingByUserId(userid);
            return _mapper.Map<List<StudentRaitingOutputModel>>(queryResult);
        }
    }
}
