using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DevEdu.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        public UserController() { }

        // api/user/{userId}/role/2
        //[HttpPost("{userId}/role/{roleId}")]
        //public int AddRoleToUser(int userId, int roleId)
        //{
        //    return 42;
        //}

        [HttpPost("User/{UserId}/Role/{RoleId}")]
        public string AddRoleToUser(int UserId, int RoleId)
        {
            return $"User {UserId} add  Role Id {RoleId}";
        }

        [HttpDelete("User/{UserId}/Role/{RoleId}")]
        public string RemoveRoleToUser(int UserId, int RoleId)
        {
            return $"User {UserId} remove  Role Id:{RoleId}";
        }
    }
}
