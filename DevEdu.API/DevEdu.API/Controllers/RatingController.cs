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
using DevEdu.API.Configuration;
using Microsoft.AspNetCore.Authorization;

namespace DevEdu.API.Controllers
{
    [Authorize]
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
        [AuthorizeRoles(Role.Teacher)]
        [HttpPost]
        [Description("Add StudentRating to database")]
        [ProducesResponseType(typeof(StudentRatingOutputModel), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ValidationExceptionResponse), StatusCodes.Status422UnprocessableEntity)]
        public ActionResult<StudentRatingOutputModel> AddStudentRating([FromBody] StudentRatingInputModel model)
        {
            var dto = _mapper.Map<StudentRatingDto>(model);
            var authorUserInfo = this.GetUserIdAndRoles();
            dto = _service.AddStudentRating(dto, authorUserInfo);
            var output = _mapper.Map<UserUpdateInfoOutPutModel>(dto);
            return StatusCode(201, output);
        }

        // api/rating/1
        [AuthorizeRoles(Role.Teacher)]
        [HttpDelete("{id}")]
        [Description("Delete StudentRating from database")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status404NotFound)]
        public ActionResult DeleteStudentRating(int id)
        {
            var authorUserInfo = this.GetUserIdAndRoles();
            _service.DeleteStudentRating(id, authorUserInfo);
            return NoContent();
        }

        // api/rating/1/{periodNumber}/value/50
        [AuthorizeRoles(Role.Teacher)]
        [HttpPut("{id}/period/{periodNumber}/value/{value}")]
        [Description("Update StudentRating in database and return updated StudentRating")]
        [ProducesResponseType(typeof(StudentRatingOutputModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status404NotFound)]
        public StudentRatingOutputModel UpdateStudentRating(int id, int value, int periodNumber)
        {
            var authorUserInfo = this.GetUserIdAndRoles();
            var dto = _service.UpdateStudentRating(id, value, periodNumber, authorUserInfo);
            return _mapper.Map<StudentRatingOutputModel>(dto);
        }

        // api/rating
        [AuthorizeRoles(Role.Manager)]
        [HttpGet]
        [Description("Get all StudentRatings from database")]
        [ProducesResponseType(typeof(List<StudentRatingOutputModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status403Forbidden)]
        public List<StudentRatingOutputModel> GetAllStudentRatings()
        {
            var dto = _service.GetAllStudentRatings();
            return _mapper.Map<List<StudentRatingOutputModel>>(dto);
        }

        // api/rating/by-group/1
        [HttpGet("by-group/{groupId}")]
        [Description("Get StudentRating from database by GroupID")]
        [ProducesResponseType(typeof(StudentRatingOutputModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status404NotFound)]
        public List<StudentRatingOutputModel> GetStudentRatingByGroupId(int groupId)
        {
            var authorUserInfo = this.GetUserIdAndRoles();
            var dto = _service.GetStudentRatingByGroupId(groupId, authorUserInfo);
            return _mapper.Map<List<StudentRatingOutputModel>>(dto);
        }

        // api/rating/by-user/1
        [AuthorizeRoles(Role.Manager)]
        [HttpGet("by-user/{userid}")]
        [Description("Get StudentRatings from database by UserID")]
        [ProducesResponseType(typeof(List<StudentRatingOutputModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status404NotFound)]
        public List<StudentRatingOutputModel> GetStudentRatingByUserId(int userid)
        {
            var dto = _service.GetStudentRatingByUserId(userid);
            return _mapper.Map<List<StudentRatingOutputModel>>(dto);
        }
    }
}