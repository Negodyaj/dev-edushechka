using AutoMapper;
using DevEdu.API.Common;
using DevEdu.API.Configuration;
using DevEdu.API.Models.InputModels;
using DevEdu.API.Models.OutputModels;
using DevEdu.Business.Services;
using DevEdu.DAL.Enums;
using DevEdu.DAL.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.ComponentModel;
using Microsoft.AspNetCore.Authorization;

namespace DevEdu.API.Controllers
{
    [Authorize]
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
        [AuthorizeRoles(Role.Manager)]
        [HttpPut("{userId}")]
        [Description("Update user")]
        [ProducesResponseType(typeof(UserUpdateInfoOutPutModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ValidationExceptionResponse), StatusCodes.Status422UnprocessableEntity)]
        public UserUpdateInfoOutPutModel UpdateUserById([FromBody] UserUpdateInputModel model)
        {
            var dtoEntry = _mapper.Map<UserDto>(model);
            var dtoResult = _userService.UpdateUser(dtoEntry);
            return _mapper.Map<UserUpdateInfoOutPutModel>(dtoResult);
        }

        // api/user/{userId}
        [AuthorizeRoles(Role.Manager)]
        [HttpGet("{userId}")]
        [Description("Return user by id")]
        [ProducesResponseType(typeof(UserFullInfoOutPutModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status404NotFound)]
        public UserFullInfoOutPutModel GetUserById(int userId)
        {
            var dto = _userService.GetUserById(userId);
            return _mapper.Map<UserFullInfoOutPutModel>(dto);
        }

        // api/user
        [AuthorizeRoles(Role.Manager)]
        [HttpGet]
        [Description("Return list users")]
        [ProducesResponseType(typeof(List<UserInfoOutPutModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status403Forbidden)]
        public List<UserInfoOutPutModel> GetAllUsers()
        {
            var listDto = _userService.GetAllUsers();
            return _mapper.Map<List<UserInfoOutPutModel>>(listDto);
        }

        // api/user/{userId}
        [AuthorizeRoles(Role.Manager)]
        [HttpDelete("{userId}")]
        [Description("Delete user by id")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status404NotFound)]
        public void DeleteUser(int userId)
        {
            _userService.DeleteUser(userId);
        }

        // api/user/{userId}/role/{roleId}
        [AuthorizeRoles()]
        [HttpPost("{userId}/role/{roleId}")]
        [Description("Add new role to user")]
        [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status404NotFound)]
        public void AddRoleToUser(int userId, int roleId)
        {
            _userService.AddUserRole(userId, (int)roleId);
        }

        // api/user/{userId}/role/{roleId}
        [AuthorizeRoles()]
        [HttpDelete("{userId}/role/{roleId}")]
        [Description("Delete role from user")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status404NotFound)]
        public void DeleteRoleFromUser(int userId, int roleId)
        {
            _userService.DeleteUserRole(userId, (int)roleId);
        }
    }
}