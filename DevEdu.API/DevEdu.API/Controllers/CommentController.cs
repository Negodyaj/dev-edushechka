using System;
using AutoMapper;
using DevEdu.API.Models;
using DevEdu.API.Common;
using DevEdu.API.Extensions;
using DevEdu.Business.Services;
using DevEdu.DAL.Enums;
using DevEdu.DAL.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using DevEdu.API.Configuration.ExceptionResponses;
using DevEdu.Business.Services.Interfaces;

namespace DevEdu.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CommentController : Controller
    {
        private readonly IMapper _mapper;
        private readonly ICommentService _commentService;

        public CommentController(IMapper mapper, ICommentService commentService)
        {
            _mapper = mapper;
            _commentService = commentService;
        }

        //  api/comment/5
        [AuthorizeRoles(Role.Teacher, Role.Tutor, Role.Student)]
        [HttpGet("{id}")]
        [Description("Return comment by id")]
        [ProducesResponseType(typeof(CommentInfoOutputModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status404NotFound)]
        public CommentInfoOutputModel GetComment(int id)
        {
            var userInfo = this.GetUserIdAndRoles();
            var dto = _commentService.GetComment(id, userInfo);
            return _mapper.Map<CommentInfoOutputModel>(dto);
        }

        //  api/comment/to-lesson/1
        [AuthorizeRoles(Role.Teacher, Role.Tutor, Role.Student)]
        [HttpPost("to-lesson/{lessonId}")]
        [Description("Add new comment to lesson")]
        [ProducesResponseType(typeof(CommentInfoOutputModel), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ValidationExceptionResponse), StatusCodes.Status422UnprocessableEntity)]
        public ActionResult<CommentInfoOutputModel> AddCommentToLesson(int lessonId, [FromBody] CommentAddInputModel model)
        {
            var userInfo = this.GetUserIdAndRoles();
            var dto = _mapper.Map<CommentDto>(model);
            var comment = _commentService.AddCommentToLesson(lessonId, dto, userInfo);
            var output = _mapper.Map<CommentInfoOutputModel>(comment);

            return Created(new Uri("api/comment/{id}", UriKind.Relative), output);
        }

        //  api/comment/to-student-answer/1
        [AuthorizeRoles(Role.Teacher, Role.Tutor, Role.Student)]
        [HttpPost("to-student-answer/{studentHomeworkId}")]
        [Description("Add new comment to student answer")]
        [ProducesResponseType(typeof(CommentInfoOutputModel), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ValidationExceptionResponse), StatusCodes.Status422UnprocessableEntity)]
        public ActionResult<CommentInfoOutputModel> AddCommentToStudentAnswer(int studentHomeworkId, [FromBody] CommentAddInputModel model)
        {
            var userInfo = this.GetUserIdAndRoles();
            var dto = _mapper.Map<CommentDto>(model);
            var comment = _commentService.AddCommentToStudentAnswer(studentHomeworkId, dto, userInfo);
            var output = _mapper.Map<CommentInfoOutputModel>(comment);

            return Created(new Uri("api/comment/{id}", UriKind.Relative), output);
        }

        //  api/comment/5
        [AuthorizeRoles(Role.Teacher, Role.Tutor, Role.Student)]
        [HttpDelete("{id}")]
        [Description("Delete comment by id")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status404NotFound)]
        public ActionResult DeleteComment(int id)
        {
            var userInfo = this.GetUserIdAndRoles();
            _commentService.DeleteComment(id, userInfo);
            return NoContent();
        }

        //  api/comment/5
        [AuthorizeRoles(Role.Teacher, Role.Tutor, Role.Student)]
        [HttpPut("{id}")]
        [Description("Update comment by id")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ValidationExceptionResponse), StatusCodes.Status422UnprocessableEntity)]
        public CommentInfoOutputModel UpdateComment(int id, [FromBody] CommentUpdateInputModel model)
        {
            var userInfo = this.GetUserIdAndRoles();
            var dto = _mapper.Map<CommentDto>(model);
            var updateDto = _commentService.UpdateComment(id, dto, userInfo);
            return _mapper.Map<CommentInfoOutputModel>(updateDto);
        }
    }
}