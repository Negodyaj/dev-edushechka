using AutoMapper;
using DevEdu.API.Common;
using DevEdu.API.Configuration.ExceptionResponses;
using DevEdu.API.Extensions;
using DevEdu.API.Models;
using DevEdu.Business.Services;
using DevEdu.DAL.Enums;
using DevEdu.DAL.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;

namespace DevEdu.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [AuthorizeRoles(Role.Teacher, Role.Manager)]
    public class RatingsController : Controller
    {
        private readonly IRatingService _service;
        private readonly IMapper _mapper;

        public RatingsController(IMapper mapper, IRatingService service)
        {
            _service = service;
            _mapper = mapper;
        }

        // api/ratings
        [AuthorizeRoles(Role.Teacher)]
        [HttpPost]
        [Description("Add StudentRating to database")]
        [ProducesResponseType(typeof(StudentRatingOutputModel), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ValidationExceptionResponse), StatusCodes.Status422UnprocessableEntity)]
        public async Task<ActionResult<StudentRatingOutputModel>> AddStudentRatingAsync([FromBody] StudentRatingInputModel model)
        {
            var dto = _mapper.Map<StudentRatingDto>(model);
            var authorUserInfo = this.GetUserIdAndRoles();
            dto = await _service.AddStudentRatingAsync(dto, authorUserInfo);
            var output = _mapper.Map<StudentRatingOutputModel>(dto);
            return Created(new Uri($"api/Rating/by-user/{output.Id}", UriKind.Relative), output);
        }

        // api/ratings/1
        [AuthorizeRoles(Role.Teacher)]
        [HttpDelete("{id}")]
        [Description("Delete StudentRating from database")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult> DeleteStudentRatingAsync(int id)
        {
            var authorUserInfo = this.GetUserIdAndRoles();
            await _service.DeleteStudentRatingAsync(id, authorUserInfo);
            return NoContent();
        }

        // api/ratings/1/{periodNumber}/value/50
        [AuthorizeRoles(Role.Teacher)]
        [HttpPut("{id}/period/{periodNumber}/value/{value}")]
        [Description("Update StudentRating in database and return updated StudentRating")]
        [ProducesResponseType(typeof(StudentRatingOutputModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status404NotFound)]
        public async Task<StudentRatingOutputModel> UpdateStudentRatingAsync(int id, int value, int periodNumber)
        {
            var authorUserInfo = this.GetUserIdAndRoles();
            var dto = await _service.UpdateStudentRatingAsync(id, value, periodNumber, authorUserInfo);
            return _mapper.Map<StudentRatingOutputModel>(dto);
        }

        // api/ratings
        [AuthorizeRoles(Role.Manager)]
        [HttpGet]
        [Description("Get all StudentRatings from database")]
        [ProducesResponseType(typeof(List<StudentRatingOutputModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status403Forbidden)]
        public async Task<List<StudentRatingOutputModel>> GetAllStudentRatingsAsync()
        {
            var dto = await _service.GetAllStudentRatingsAsync();
            return _mapper.Map<List<StudentRatingOutputModel>>(dto);
        }

        // api/ratings/by-group/{groupId}
        [HttpGet("by-group/{groupId}")]
        [Description("Get StudentRating from database by GroupID")]
        [ProducesResponseType(typeof(StudentRatingOutputModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status404NotFound)]
        public async Task<List<StudentRatingOutputModel>> GetStudentRatingByGroupIdAsync(int groupId)
        {
            var authorUserInfo = this.GetUserIdAndRoles();
            var dto = await _service.GetStudentRatingByGroupIdAsync(groupId, authorUserInfo);
            return _mapper.Map<List<StudentRatingOutputModel>>(dto);
        }

        // api/ratings/by-user/1
        [AuthorizeRoles(Role.Manager)]
        [HttpGet("by-user/{userid}")]
        [Description("Get StudentRatings from database by UserID")]
        [ProducesResponseType(typeof(List<StudentRatingOutputModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status404NotFound)]
        public async Task<List<StudentRatingOutputModel>> GetStudentRatingByUserIdAsync(int userid)
        {
            var dto = await _service.GetStudentRatingByUserIdAsync(userid);
            return _mapper.Map<List<StudentRatingOutputModel>>(dto);
        }
    }
}