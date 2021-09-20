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
using System.ComponentModel;
using System.Threading.Tasks;

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
        public async Task<CommentInfoOutputModel> GetCommentAsync(int id)
        {
            var userInfo = this.GetUserIdAndRoles();
            var dto = await _commentService.GetCommentAsync(id, userInfo);
            var comment = _mapper.Map<CommentInfoOutputModel>(dto);

            return comment;
        }

        //  api/comment/to-lesson/1
        [AuthorizeRoles(Role.Teacher, Role.Tutor, Role.Student)]
        [HttpPost("to-lesson/{lessonId}")]
        [Description("Add new comment to lesson")]
        [ProducesResponseType(typeof(CommentInfoOutputModel), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ValidationExceptionResponse), StatusCodes.Status422UnprocessableEntity)]
        public async Task<ActionResult<CommentInfoOutputModel>> AddCommentToLessonAsync(int lessonId, [FromBody] CommentInputModel model)
        {
            var userInfo = this.GetUserIdAndRoles();
            var dto = _mapper.Map<CommentDto>(model);
            var comment = await _commentService.AddCommentToLessonAsync(lessonId, dto, userInfo);
            var output = _mapper.Map<CommentInfoOutputModel>(comment);

            return Created(new Uri($"api/Comment/{output.Id}", UriKind.Relative), output);
        }

        //  api/comment/to-student-answer/1
        [AuthorizeRoles(Role.Teacher, Role.Tutor, Role.Student)]
        [HttpPost("to-student-answer/{studentHomeworkId}")]
        [Description("Add new comment to student answer")]
        [ProducesResponseType(typeof(CommentInfoOutputModel), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ValidationExceptionResponse), StatusCodes.Status422UnprocessableEntity)]
        public async Task<ActionResult<CommentInfoOutputModel>> AddCommentToStudentAnswerAsync(int studentHomeworkId, [FromBody] CommentInputModel model)
        {
            var userInfo = this.GetUserIdAndRoles();
            var dto = _mapper.Map<CommentDto>(model);
            var comment = await _commentService.AddCommentToStudentAnswerAsync(studentHomeworkId, dto, userInfo);
            var output = _mapper.Map<CommentInfoOutputModel>(comment);

            return Created(new Uri($"api/Comment/{output.Id}", UriKind.Relative), output);
        }

        //  api/comment/5
        [AuthorizeRoles(Role.Teacher, Role.Tutor, Role.Student)]
        [HttpDelete("{id}")]
        [Description("Delete comment by id")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult> DeleteCommentAsync(int id)
        {
            var userInfo = this.GetUserIdAndRoles();
            await _commentService.DeleteCommentAsync(id, userInfo);

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
        public async Task<CommentInfoOutputModel> UpdateCommentAsync(int id, [FromBody] CommentInputModel model)
        {
            var userInfo = this.GetUserIdAndRoles();
            var dto = _mapper.Map<CommentDto>(model);
            var updateDto = await _commentService.UpdateCommentAsync(id, dto, userInfo);
            var comment =  _mapper.Map<CommentInfoOutputModel>(updateDto);

            return comment;
        }
    }
}