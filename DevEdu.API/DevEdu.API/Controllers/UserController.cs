using AutoMapper;
using DevEdu.API.Models.InputModels;
using DevEdu.API.Models.OutputModels;
using DevEdu.Business.Servicies;
using DevEdu.DAL.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.ComponentModel;

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
        [Description("Add new user")]
        [ProducesResponseType(typeof(int), StatusCodes.Status201Created)]
        public int AddUser([FromBody] UserInsertInputModel model)
        {
            var dto = _mapper.Map<UserDto>(model);
            return _userService.AddUser(dto);
        }

        // api/user/userId
        [HttpPut("{userId}")]
        [Description("Update user")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public void UpdateUserById([FromBody] UserUpdateInputModel model)
        {
            var dto = _mapper.Map<UserDto>(model);
            _userService.UpdateUser(dto);
        }

        // api/user/{userId}
        [HttpGet("{userId}")]
        [Description("Return user by id")]
        [ProducesResponseType(typeof(UserFullInfoOutPutModel), StatusCodes.Status200OK)]
        public UserFullInfoOutPutModel GetUserById(int userId)
        {
            var dto = _userService.SelectUserById(userId);
            return _mapper.Map<UserFullInfoOutPutModel>(dto);
        }

        // api/user
        [HttpGet]
        [Description("Return list users")]
        [ProducesResponseType(typeof(List<UserInfoOutPutModel>), StatusCodes.Status200OK)]
        public List<UserInfoOutPutModel> GetAllUsers()
        {
            var listDto = _userService.SelectUsers();
            return _mapper.Map<List<UserInfoOutPutModel>>(listDto);
        }

        // api/user/{userId}
        [HttpDelete("{userId}")]
        [Description("Delete user by id")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public void DeleteUser(int userId)
        {
            _userService.DeleteUser(userId);
        }

        // api/user/{userId}/role/{roleId}
        [HttpPost("{userId}/role/{roleId}")]
        [Description("Add new role to user")]
        [ProducesResponseType(typeof(int), StatusCodes.Status201Created)]
        public int AddRoleToUser(int userId, int roleId)
        {
            return _userService.AddUserRole(userId, roleId);
        }

        // api/user/{userId}/role/{roleId}
        [HttpDelete("{userId}/role/{roleId}")]
        [Description("Delete role from user")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public void DeleteRoleFromUser(int userId, int roleId)
        {
            _userService.DeleteUserRole(userId, roleId);
        }
    }
}