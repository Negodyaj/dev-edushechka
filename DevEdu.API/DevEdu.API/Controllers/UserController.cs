using DevEdu.API.Models.InputModels;
using Microsoft.AspNetCore.Mvc;

namespace DevEdu.API.Controllers
{
    [ApiController]
    [Route("api/user")]
    public class UserController : Controller
    {
        UserInputModel model = new UserInputModel();

        public UserController() { }

        [HttpPost]
        public int AddUser([FromBody] UserInputModel model)
        {
            return 1;
        }

        [HttpGet("{UserId}")]
        public int GetUserById(int UserId)
        {

            return UserId;
        }

        [HttpDelete("{UserId}")]
        public string DeleteUser(int UserId)
        {
            return $"User with number {UserId} is deleted";
        }

        // api/user/{userId}/role/2
        [HttpPost("{userId}/role/{roleId}")]
        public int AddRoleToUser(int userId, int roleId)
        {
            return 42;
        }
    }
}
