﻿using DevEdu.API.Models.InputModels;
using Microsoft.AspNetCore.Mvc;

namespace DevEdu.API.Controllers
{
    [ApiController]
    [Route("api/user")]
    public class UserController : Controller
    {
        public UserController() { }

        [HttpPost]
        public int AddUser([FromBody] UserInputModel model)
        {
            return 1;
        }

        [HttpGet("{UserId}")]
        public string GetUserById(int UserId)
        {
            return UserId.ToString();
        }

        [HttpGet("all")]
        public string GetAllUsers()
        {
            return "here's for you all Users";
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
