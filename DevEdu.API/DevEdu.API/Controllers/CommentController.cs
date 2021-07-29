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
        [HttpGet("{id}")]
        [Description("Return comment by id")]
        [ProducesResponseType(typeof(CommentInfoOutputModel), StatusCodes.Status200OK)]
        public CommentInfoOutputModel GetComment(int id)
        {
            var dto = _commentService.GetComment(id);
            var output = _mapper.Map<CommentInfoOutputModel>(dto);
            return output;
        }

        //  api/comment/by-user/1
        [HttpGet("by-user/{userId}")]
        [Description("Return comments by user")]
        [ProducesResponseType(typeof(List<CommentInfoOutputModel>), StatusCodes.Status200OK)]
        public List<CommentInfoOutputModel> GetCommentsByUserId(int userId)
        {
            var dto = _commentService.GetCommentsByUserId(userId);
            var output = _mapper.Map<List<CommentInfoOutputModel>>(dto);
            return output;
        }

        //  api/comment
        [HttpPost]
        [Description("Add new comment")]
        [ProducesResponseType(typeof(int), StatusCodes.Status201Created)]
        public int AddComment([FromBody] CommentAddInputModel model)
        {
            var dto = _mapper.Map<CommentDto>(model);
            return _commentService.AddComment(dto);
        }

        //  api/comment/5
        [HttpDelete("{id}")]
        [Description("Delete comment by id")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public void DeleteComment(int id)
        {
            _commentService.DeleteComment(id);
        }

        //  api/comment/5
        [HttpPut("{id}")]
        [Description("Update comment by id")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public CommentInfoOutputModel UpdateComment(int id, [FromBody] CommentUpdateInputModel model)
        {
            var dto = _mapper.Map<CommentDto>(model);
            var output = _commentService.UpdateComment(id, dto);
            return _mapper.Map<CommentInfoOutputModel>(output);
        }
    }
}