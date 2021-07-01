using System.Collections.Generic;
using DevEdu.API.Models.InputModels;
using DevEdu.DAL.Models;
using DevEdu.DAL.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace DevEdu.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CommentController : Controller
    {
        MappersController mappersController = new MappersController();
        CommentRepository commentRepository = new CommentRepository();
        public CommentController()
        {
        }

        //  api/comment/5
        [HttpGet("{id}")]
        public CommentDto GetComment(int id)
        {
            return commentRepository.GetComment(id);
        }

        //  api/comment/by-user/1
        [HttpGet("by-user/{userId}")]
        public List<CommentDto> GetAllCommentsByUserId(int userId)
        {
            return commentRepository.GetCommentsByUser(userId);
        }

        //  api/comment
        [HttpPost]
        public int AddComment([FromBody] CommentAddtInputModel model)
        {
            int id = commentRepository.AddComment(mappersController.MapCommentModelToDto(model));
            return id;
        }

        //  api/comment/5
        [HttpDelete("{id}")]
        public void DeleteComment(int id)
        {
            commentRepository.DeleteComment(id);
        }

        //  api/comment/5
        [HttpPut("{id}")]
        public string UpdateComment(int id, [FromBody] CommentUpdatetInputModel model)
        {
            commentRepository.UpdateComment(id, (mappersController.MapCommentModelToDto(model)));
            return $"Text comment №{id} change to {model.Text}";
        }
    }
}