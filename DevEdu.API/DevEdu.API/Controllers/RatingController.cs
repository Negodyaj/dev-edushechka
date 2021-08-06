using AutoMapper;
using DevEdu.API.Common;
using DevEdu.API.Extensions;
using DevEdu.API.Models.InputModels;
using DevEdu.API.Models.OutputModels;
using DevEdu.Business.Services;
using DevEdu.DAL.Enums;
using DevEdu.DAL.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.ComponentModel;

namespace DevEdu.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [AuthorizeRoles(Role.Teacher, Role.Manager)]
    public class RatingController : Controller
    {
        private IRatingService _service;

        private readonly IMapper _mapper;

        public RatingController(IMapper mapper, IRatingService service)
        {
            _service = service;
            _mapper = mapper;
        }

        // api/rating
        [HttpPost]
        [Description("Add StudentRating to database")]
        [ProducesResponseType(typeof(StudentRatingOutputModel), StatusCodes.Status201Created)]
        [AuthorizeRoles(Role.Teacher)]
        public StudentRatingOutputModel AddStudentRating([FromBody] StudentRatingInputModel model)
        {
            var dto = _mapper.Map<StudentRatingDto>(model);
            var authorUserId = this.GetUserId();
            dto = _service.AddStudentRating(dto, authorUserId);
            return _mapper.Map<StudentRatingOutputModel>(dto);
        }

        // api/rating/1
        [HttpDelete("{id}")]
        [Description("Delete StudentRating from database")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [AuthorizeRoles(Role.Teacher)]
        public void DeleteStudentRating(int id)
        {
            var authorUserId = this.GetUserId();
            _service.DeleteStudentRating(id, authorUserId);
        }

        // api/rating/1/{periodNumber}/value/50
        [HttpPut("{id}/period/{periodNumber}/value/{value}")]
        [Description("Update StudentRating in database and return updated StudentRating")]
        [ProducesResponseType(typeof(StudentRatingOutputModel), StatusCodes.Status200OK)]
        [AuthorizeRoles(Role.Teacher)]
        public StudentRatingOutputModel UpdateStudentRating(int id, int value, int periodNumber)
        {
            var authorUserId = this.GetUserId();
            var dto = _service.UpdateStudentRating(id, value, periodNumber, authorUserId);
            return _mapper.Map<StudentRatingOutputModel>(dto);
        }

        // api/rating
        [HttpGet]
        [Description("Get all StudentRatings from database")]
        [ProducesResponseType(typeof(List<StudentRatingOutputModel>), StatusCodes.Status200OK)]
        [AuthorizeRoles(Role.Manager)]
        public List<StudentRatingOutputModel> GetAllStudentRatings()
        {
            var dto = _service.GetAllStudentRatings();
            return _mapper.Map<List<StudentRatingOutputModel>>(dto);
        }


        // api/rating/by-group/1
        [HttpGet("by-group/{groupId}")]
        [Description("Get StudentRating from database by GroupID")]
        [ProducesResponseType(typeof(StudentRatingOutputModel), StatusCodes.Status200OK)]
        public List<StudentRatingOutputModel> GetStudentRatingByGroupID(int groupId)
        {
            var authorUserId = this.GetUserId();
            var authRoles = this.GetUserRoles();
            var dto = _service.GetStudentRatingByGroupId(groupId, authorUserId, authRoles);
            return _mapper.Map<List<StudentRatingOutputModel>>(dto);
        }

        // api/rating/by-user/1
        [HttpGet("by-user/{userid}")]
        [Description("Get StudentRatings from database by UserID")]
        [ProducesResponseType(typeof(List<StudentRatingOutputModel>), StatusCodes.Status200OK)]
        [AuthorizeRoles(Role.Manager)]
        public List<StudentRatingOutputModel> GetStudentRatingByUserId(int userid)
        {
            var dto = _service.GetStudentRatingByUserId(userid);
            return _mapper.Map<List<StudentRatingOutputModel>>(dto);
        }
    }
}
