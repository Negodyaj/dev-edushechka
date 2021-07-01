using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
        public CommentController()
        {
        }

        //  api/comment/5
        [HttpGet("{id}")]
        public string GetComment(int id)
        {
            return $"Comment №{id}";
        }

        //  api/comment/by-user/1
        [HttpGet("by-user/{userId}")]
        public List<CommentDto> GetAllCommentsByUserId(int userId)
        {
            return new List<CommentDto>();
        }

        //  api/comment
        [HttpPost]
        public int AddComment([FromBody] CommentAddtInputModel model)
        {
            MappersController mappersController=new MappersController();
            CommentRepository commentRepository = new CommentRepository();
            int id = commentRepository.AddComment(mappersController.MapCommentModelToDto(model));
            return id;
        }

        //  api/comment/5
        [HttpDelete("{id}")]
        public void DeleteComment(int id)
        {

        }

        //  api/comment/5
        [HttpPut("{id}")]
        public string UpdateComment(int id, [FromBody] CommentUpdatetInputModel model)
        {
            return $"Text comment №{id} change to {model.Text}";
        }
    }
}