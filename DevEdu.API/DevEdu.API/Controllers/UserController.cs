using AutoMapper;
using DevEdu.API.Models.InputModels;
using DevEdu.Business.Servicies;
using DevEdu.DAL.Models;
using DevEdu.DAL.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace DevEdu.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IUserService _userService;

        public UserController(IMapper mapper, IUserService userService)
        {
            _mapper = mapper;
            _userService = userService;
        }

        // api/user
        [HttpPost]
        public int AddUser([FromBody] UserInsertInputModel model)
        {
            var dto = _mapper.Map<UserDto>(model);
            return _userService.AddUser(dto);
        }

        // api/user/userId
        [HttpPut("{userId}")]
        public void UpdateUserById([FromBody] UserUpdateInputModel model)
        {
            var dto = _mapper.Map<UserDto>(model);
            _userService.UpdateUser(dto);
        }

        // api/user/{userId}
        [HttpGet("{userId}")]
        public UserDto GetUserById(int userId)
        {
            return _userService.SelectUserById(userId);
        }

        // api/user
        [HttpGet]
        public Dictionary<int, UserDto> GetAllUsers()
        {
            return _userService.SelectUsers();
        }

        // api/user/{userId}
        [HttpDelete("{userId}")]
        public void DeleteUser(int userId)
        {
            _userService.DeleteUser(userId);
        }

        // api/user/{userId}/role/{roleId}
        [HttpPost("{userId}/role/{roleId}")]
        public int AddRoleToUser(int userId, int roleId)
        {
            return _userService.AddUserRole(userId, roleId);
        }

        // api/user/{userId}/role/{roleId}
        [HttpDelete("{userId}/role/{roleId}")]
        public void DeleteRoleFromUser(int userId, int roleId)
        {
            _userService.DeleteUserRole(userId, roleId);
        }
    }
}