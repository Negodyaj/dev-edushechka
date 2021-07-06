using System.Collections.Generic;
using AutoMapper;
using DevEdu.API.Models.InputModels;
using DevEdu.Business.Servicies;
using DevEdu.DAL.Models;
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
        [HttpGet("{id}")]
        public CommentDto GetComment(int id)
        {
            return _commentService.GetComment(id);
        }

        //  api/comment/by-user/1
        [HttpGet("by-user/{userId}")]
        public List<CommentDto> GetCommentsByUserId(int userId)
        {
            return _commentService.GetCommentsByUserId(userId);
        }

        //  api/comment
        [HttpPost]
        public int AddComment([FromBody] CommentAddInputModel model)
        {
            var dto = _mapper.Map<CommentDto>(model);
            return _commentService.AddComment(dto);
        }

        //  api/comment/5
        [HttpDelete("{id}")]
        public void DeleteComment(int id)
        {
            _commentService.DeleteComment(id);
        }

        //  api/comment/5
        [HttpPut("{id}")]
        public string UpdateComment(int id, [FromBody] CommentUpdateInputModel model)
        {
            var dto = _mapper.Map<CommentDto>(model);
            _commentService.UpdateComment(id, dto);
            return $"Text comment №{id} change to {model.Text}";
        }
    }
}