using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DevEdu.API.Models.InputModels;
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

        //  api/comment/all/user/1
        [HttpGet("all/byUser/{userId}")]
        public string GetAllComment(int userId)
        {
            return $"All comments by user №{userId}";
        }

        //  api/comment
        [HttpPost]
        public int AddComment([FromBody] CommentInputModel model)
        {
            return 1;
        }

        //  api/comment/{id}
        [HttpDelete("{id}")]
        public void DeleteComment(int id)
        {

        }

        //  api/comment/{id}
        [HttpPut("{id}")]
        public string UpdateComment(int id, string textComment)
        {
            return $"Text comment №{id} change to {textComment}";
        }
    }
}