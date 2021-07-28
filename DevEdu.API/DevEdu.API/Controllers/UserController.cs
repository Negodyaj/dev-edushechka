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

        // api/user/userId
        [HttpPut("{userId}")]
        [Description("Update user")]
        [AuthorizeRoles(Role.Manager)]
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
        [AuthorizeRoles(Role.Manager)]
        [ProducesResponseType(typeof(UserFullInfoOutPutModel), StatusCodes.Status200OK)]
        public UserFullInfoOutPutModel GetUserById(int userId)
        {
            var dto = _userService.SelectUserById(userId);
            return _mapper.Map<UserFullInfoOutPutModel>(dto);
        }

        // api/user
        [HttpGet]
        [Description("Return list users")]
        [AuthorizeRoles(Role.Manager)]
        [ProducesResponseType(typeof(List<UserInfoOutPutModel>), StatusCodes.Status200OK)]
        public List<UserInfoOutPutModel> GetAllUsers()
        {
            var listDto = _userService.SelectUsers();
            return _mapper.Map<List<UserInfoOutPutModel>>(listDto);
        }

        // api/user/{userId}
        [HttpDelete("{userId}")]
        [Description("Delete user by id")]
        [AuthorizeRoles(Role.Manager)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public void DeleteUser(int userId)                       
        {
            _userService.DeleteUser(userId);
        }

        // api/user/{userId}/role/{roleId}
        [HttpPost("{userId}/role/{roleId}")]
        [Description("Add new role to user")]
        [AuthorizeRoles()]
        [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
        public void AddRoleToUser(int userId, Role roleId)
        {
            _userService.AddUserRole(userId, (int)roleId);
        }

        // api/user/{userId}/role/{roleId}
        [HttpDelete("{userId}/role/{roleId}")]
        [Description("Delete role from user")]
        [AuthorizeRoles()]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public void DeleteRoleFromUser(int userId, Role roleId)
        {
            _userService.DeleteUserRole(userId, (int)roleId);
        }
    }
}