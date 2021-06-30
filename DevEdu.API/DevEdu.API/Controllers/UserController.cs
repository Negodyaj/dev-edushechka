using DevEdu.API.Models.InputModels;
using Microsoft.AspNetCore.Mvc;

namespace DevEdu.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        public UserController() { }

        // api/user
        [HttpPost]
        public int AddUser([FromBody] UserInputModel model)
        {
            return 1;
        }

        // api/user/{userId}
        [HttpGet("{userId}")]
        public string GetUserById(int userId)
        {
            return userId.ToString();
        }

        // api/user
        [HttpGet]
        public string GetAllUsers()
        {
            return "here's for you all Users";
        }

        // api/user/{userId}
        [HttpDelete("{userId}")]
        public string DeleteUser(int userId)
        {
            return $"User with number {userId} is deleted";
        }

        // api/user/{userId}/role/{roleId}
        [HttpPost("{userId}/role/{roleId}")]
        public int AddRoleToUser(int userId, int roleId)
        {
            return 42;
        }
    }
}
