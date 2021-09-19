using AutoMapper;
using DevEdu.API.Common;
using DevEdu.API.Configuration.ExceptionResponses;
using DevEdu.API.Models;
using DevEdu.Business.Services;
using DevEdu.DAL.Enums;
using DevEdu.DAL.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;

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
        [AuthorizeRoles(Role.Manager)]
        [HttpPut("{userId}")]
        [Description("Update user")]
        [ProducesResponseType(typeof(UserUpdateInfoOutPutModel), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ValidationExceptionResponse), StatusCodes.Status422UnprocessableEntity)]
        public async Task<ActionResult<UserUpdateInfoOutPutModel>> UpdateUserById([FromBody] UserUpdateInputModel model)
        {
            var dtoEntry = _mapper.Map<UserDto>(model);
            var dtoResult = await _userService.UpdateUser(dtoEntry);
            var outPut = _mapper.Map<UserUpdateInfoOutPutModel>(dtoResult);

            return Created(new Uri($"api/User/{outPut.Id}", UriKind.Relative), outPut);
        }

        // api/user/{userId}
        [AuthorizeRoles(Role.Manager)]
        [HttpGet("{userId}")]
        [Description("Return user by id")]
        [ProducesResponseType(typeof(UserFullInfoOutPutModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status404NotFound)]
        public async Task<UserFullInfoOutPutModel> GetUserById(int userId)
        {
            var dto = await _userService.GetUserById(userId);
            var outPut = _mapper.Map<UserFullInfoOutPutModel>(dto);

            return outPut;
        }

        // api/user
        [AuthorizeRoles(Role.Manager)]
        [HttpGet]
        [Description("Return list users")]
        [ProducesResponseType(typeof(List<UserInfoOutPutModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status403Forbidden)]
        public async Task<List<UserInfoOutPutModel>> GetAllUsers()
        {
            var listDto = await _userService.GetAllUsers();
            var list = _mapper.Map<List<UserInfoOutPutModel>>(listDto);

            return list;
        }

        // api/user/{userId}
        [AuthorizeRoles(Role.Manager)]
        [HttpDelete("{userId}")]
        [Description("Delete user by id")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult> DeleteUserAsync(int userId)
        {
            await _userService.DeleteUser(userId);
            return NoContent();
        }

        // api/user/{userId}/role/{roleId}
        [AuthorizeRoles()]
        [HttpPost("{userId}/role/{role}")]
        [Description("Add new role to user")]
        [ProducesResponseType(typeof(int), StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult> AddRoleToUserAsync(int userId, Role role)
        {
            await _userService.AddUserRole(userId, (int)role);
            return NoContent();
        }

        // api/user/{userId}/role/{roleId}
        [AuthorizeRoles()]
        [HttpDelete("{userId}/role/{role}")]
        [Description("Delete role from user")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult> DeleteRoleFromUserAsync(int userId, Role role)
        {
            await _userService.DeleteUserRole(userId, (int)role);
            return NoContent();
        }
    }
}