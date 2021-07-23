using AutoMapper;
using DevEdu.API.Common;
using DevEdu.API.Models.InputModels;
using DevEdu.API.Models.OutputModels;
using DevEdu.Business.Services;
using DevEdu.DAL.Enums;
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
        [AuthorizeRolesAttribute(Role.Manager)]
        [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
        public int AddUser([FromBody] UserInsertInputModel model)
        {
            var dto = _mapper.Map<UserDto>(model);
            return _userService.AddUser(dto);
        }

        // api/user/userId
        [HttpPut("{userId}")]
        [Description("Update user")]
        [AuthorizeRolesAttribute(Role.Manager)]
        [ProducesResponseType(typeof(UserUpdateInfoOutPutModel), StatusCodes.Status200OK)]
        public UserUpdateInfoOutPutModel UpdateUserById([FromBody] UserUpdateInputModel model)
        {
            var dtoEntry = _mapper.Map<UserDto>(model);
            var dtoResult = _userService.UpdateUser(dtoEntry);
            return _mapper.Map<UserUpdateInfoOutPutModel>(dtoResult);
        }

        // api/user/{userId}
        [HttpGet("{userId}")]
        [Description("Return user by id")]
        [AuthorizeRolesAttribute(Role.Manager)]
        [ProducesResponseType(typeof(UserFullInfoOutPutModel), StatusCodes.Status200OK)]
        public UserFullInfoOutPutModel GetUserById(int userId)
        {
            var dto = _userService.SelectUserById(userId);
            return _mapper.Map<UserFullInfoOutPutModel>(dto);
        }

        // api/user
        [HttpGet]
        [Description("Return list users")]
        [AuthorizeRolesAttribute(Role.Manager)]
        [ProducesResponseType(typeof(List<UserInfoOutPutModel>), StatusCodes.Status200OK)]
        public List<UserInfoOutPutModel> GetAllUsers()
        {
            var listDto = _userService.SelectUsers();
            return _mapper.Map<List<UserInfoOutPutModel>>(listDto);
        }

        // api/user/{userId}
        [HttpDelete("{userId}")]
        [Description("Delete user by id")]
        [AuthorizeRolesAttribute(Role.Manager)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public void DeleteUser(int userId)
        {
            _userService.DeleteUser(userId);
        }

        // api/user/{userId}/role/{roleId}
        [HttpPost("{userId}/role/{roleId}")]
        [Description("Add new role to user")]
        [AuthorizeRolesAttribute(Role.Admin)]
        [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
        public void AddRoleToUser(int userId, int roleId)
        {
            _userService.AddUserRole(userId, roleId);
        }

        // api/user/{userId}/role/{roleId}
        [HttpDelete("{userId}/role/{roleId}")]
        [Description("Delete role from user")]
        [AuthorizeRolesAttribute(Role.Admin)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public void DeleteRoleFromUser(int userId, int roleId)
        {
            _userService.DeleteUserRole(userId, roleId);
        }
    }
}