using System.Collections.Generic;
using System.ComponentModel;
using AutoMapper;
using DevEdu.API.Models.InputModels;
using DevEdu.API.Models.OutputModels;
using DevEdu.Business.Services;
using DevEdu.DAL.Models;
using Microsoft.AspNetCore.Authorization;
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
        [Authorize]
        [HttpGet("{id}")]
        [Description("Return comment by id")]
        [ProducesResponseType(typeof(CommentInfoOutputModel), StatusCodes.Status200OK)]
        public CommentInfoOutputModel GetComment(int id)
        {
            var dto = _commentService.GetComment(id);
            var output = _mapper.Map<CommentInfoOutputModel>(dto);
            return output;
        }

        //  api/comment/to-lesson/1
        [Authorize]
        [HttpPost("to-lesson/{lessonId}")]
        [Description("Add new comment to lesson")]
        [ProducesResponseType(typeof(CommentInfoOutputModel), StatusCodes.Status201Created)]
        public CommentInfoOutputModel AddCommentToLesson(int lessonId, [FromBody] CommentAddInputModel model)
        {
            var dto = _mapper.Map<CommentDto>(model);
            var comment = _commentService.AddCommentToLesson(lessonId, dto);
            var output = _mapper.Map<CommentInfoOutputModel>(comment);
            return output;
        }

        //  api/comment/to-student-answer/1
        [Authorize]
        [HttpPost("to-student-answer/{taskStudentId}")]
        [Description("Add new comment to student answer")]
        [ProducesResponseType(typeof(CommentInfoOutputModel), StatusCodes.Status201Created)]
        public CommentInfoOutputModel AddCommentToStudentAnswer(int taskStudentId, [FromBody] CommentAddInputModel model)
        {
            var dto = _mapper.Map<CommentDto>(model);
            var comment = _commentService.AddCommentToStudentAnswer(taskStudentId, dto);
            var output = _mapper.Map<CommentInfoOutputModel>(comment);
            return output;
        }

        //  api/comment/5
        [Authorize]
        [HttpDelete("{id}")]
        [Description("Delete comment by id")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public void DeleteComment(int id)
        {
            _commentService.DeleteComment(id);
        }

        //  api/comment/5
        [Authorize]
        [HttpPut("{id}")]
        [Description("Update comment by id")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public CommentInfoOutputModel UpdateComment(int id, [FromBody] CommentUpdateInputModel model)
        {
            var dto = _mapper.Map<CommentDto>(model);
            var output= _commentService.UpdateComment(id, dto);
            return _mapper.Map<CommentInfoOutputModel>(output);
        }
    }
}