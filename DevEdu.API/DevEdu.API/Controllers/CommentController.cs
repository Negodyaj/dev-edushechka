using System.ComponentModel;
using AutoMapper;
using DevEdu.API.Common;
using DevEdu.API.Models.InputModels;
using DevEdu.API.Models.OutputModels;
using DevEdu.Business.Services;
using DevEdu.API.Extensions;
using DevEdu.DAL.Enums;
using DevEdu.DAL.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
        public CommentInfoOutputModel GetComment(int id)
        {
            var userToken = this.GetUserIdAndRoles();
            var dto = _commentService.GetComment(id, userToken);
            var output = _mapper.Map<CommentInfoOutputModel>(dto);
            return output;
        }

        //  api/comment/to-lesson/1
        [AuthorizeRoles(Role.Teacher, Role.Tutor, Role.Student)]
        [HttpPost("to-lesson/{lessonId}")]
        [Description("Add new comment to lesson")]
        [ProducesResponseType(typeof(CommentInfoOutputModel), StatusCodes.Status201Created)]
        public CommentInfoOutputModel AddCommentToLesson(int lessonId, [FromBody] CommentAddInputModel model)
        {
            var userToken = this.GetUserIdAndRoles();
            var dto = _mapper.Map<CommentDto>(model);
            var comment = _commentService.AddCommentToLesson(lessonId, dto, userToken);
            var output = _mapper.Map<CommentInfoOutputModel>(comment);
            return output;
        }

        //  api/comment/to-student-answer/1
        [AuthorizeRoles(Role.Teacher, Role.Tutor, Role.Student)]
        [HttpPost("to-student-answer/{taskStudentId}")]
        [Description("Add new comment to student answer")]
        [ProducesResponseType(typeof(CommentInfoOutputModel), StatusCodes.Status201Created)]
        public CommentInfoOutputModel AddCommentToStudentAnswer(int taskStudentId, [FromBody] CommentAddInputModel model)
        {
            var userToken = this.GetUserIdAndRoles();
            var dto = _mapper.Map<CommentDto>(model);
            var comment = _commentService.AddCommentToStudentAnswer(taskStudentId, dto, userToken);
            var output = _mapper.Map<CommentInfoOutputModel>(comment);
            return output;
        }

        //  api/comment/5
        [AuthorizeRoles(Role.Teacher, Role.Tutor, Role.Student)]
        [HttpDelete("{id}")]
        [Description("Delete comment by id")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public void DeleteComment(int id)
        {
            var userToken = this.GetUserIdAndRoles();
            _commentService.DeleteComment(id, userToken);
        }

        //  api/comment/5
        [AuthorizeRoles(Role.Teacher, Role.Tutor, Role.Student)]
        [HttpPut("{id}")]
        [Description("Update comment by id")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public CommentInfoOutputModel UpdateComment(int id, [FromBody] CommentUpdateInputModel model)
        {
            var userToken = this.GetUserIdAndRoles();
            var dto = _mapper.Map<CommentDto>(model);
            var output= _commentService.UpdateComment(id, dto, userToken);
            return _mapper.Map<CommentInfoOutputModel>(output);
        }
    }
}