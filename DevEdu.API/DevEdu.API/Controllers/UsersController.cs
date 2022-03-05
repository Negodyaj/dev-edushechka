﻿using AutoMapper;
using DevEdu.API.Common;
using DevEdu.API.Configuration.ExceptionResponses;
using DevEdu.API.Extensions;
using DevEdu.API.Models;
using DevEdu.Business.Exceptions;
using DevEdu.Business.Services;
using DevEdu.DAL.Enums;
using DevEdu.DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Threading.Tasks;

namespace DevEdu.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IUserService _userService;
        private readonly IAuthenticationService _authenticationService;
        private readonly List<string> _acceptableFileExtensions;

        public UsersController(IMapper mapper, IUserService userService, IAuthenticationService authenticationService)
        {
            _mapper = mapper;
            _userService = userService;
            _authenticationService = authenticationService;
            _acceptableFileExtensions = new List<string> { ".jpg", ".jpeg", ".png" };
        }

        // api/users/5
        [AuthorizeRoles(Role.Manager)]
        [HttpPut("{userId}")]
        [Description("Update user")]
        [ProducesResponseType(typeof(UserUpdateInfoOutPutModel), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ValidationExceptionResponse), StatusCodes.Status422UnprocessableEntity)]
        public async Task<ActionResult<UserUpdateInfoOutPutModel>> UpdateUserByIdAsync([FromBody] UserUpdateInputModel model)
        {
            var dtoEntry = _mapper.Map<UserDto>(model);
            var dtoResult = await _userService.UpdateUserAsync(dtoEntry);
            var outPut = _mapper.Map<UserUpdateInfoOutPutModel>(dtoResult);

            return Created(new Uri($"api/User/{outPut.Id}", UriKind.Relative), outPut);
        }

        // api/users/5
        [AuthorizeRoles(Role.Manager)]
        [HttpGet("{userId}")]
        [Description("Return user by id")]
        [ProducesResponseType(typeof(UserFullInfoOutPutModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status404NotFound)]
        public async Task<UserFullInfoOutPutModel> GetUserByIdAsync(int userId)
        {
            var dto = await _userService.GetUserByIdAsync(userId);
            var outPut = _mapper.Map<UserFullInfoOutPutModel>(dto);

            return outPut;
        }

        // api/users
        [AuthorizeRoles(Role.Manager)]
        [HttpGet]
        [Description("Return list users")]
        [ProducesResponseType(typeof(List<UserInfoOutPutModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status403Forbidden)]
        public async Task<List<UserInfoOutPutModel>> GetAllUsersAsync()
        {
            var listDto = await _userService.GetAllUsersAsync();
            var list = _mapper.Map<List<UserInfoOutPutModel>>(listDto);

            return list;
        }

        // api/users/password 
        [Authorize]
        [HttpPut("password")]
        [Description("Change user password")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult> ChangeUserPasswordAsync([FromBody] UserChangePasswordInputModel changePasswordInputModel)
        {
            var userId = this.GetUserId();
            var user = await _userService.GetUserByIdAsync(userId);

            if (!await _authenticationService.VerifyAsync(user.Password, changePasswordInputModel.OldPassword))
                throw new AuthorizationException("Entered old password is wrong");

            user.Password = await _authenticationService.HashPasswordAsync(changePasswordInputModel.NewPassword);
            await _userService.ChangePasswordUserAsync(user);

            return NoContent();
        }

        [Authorize]
        [HttpPost("photo")]
        [Description("Upload photo for current user (jpg,png)")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ValidationExceptionResponse), StatusCodes.Status422UnprocessableEntity)]
        public async Task<ActionResult> UpdatePhoto(IFormFile photo)
        {
            if (photo == null)
            {
                throw new ValidationException(nameof(photo), ValidationMessage.PhotoRequired);
            }
            var extension = Path.GetExtension(photo.FileName);
            if (!_acceptableFileExtensions.Contains(extension))
            {
                throw new ValidationException(nameof(photo), ValidationMessage.PhotoWrongExtension);
            }

            var userId = this.GetUserId();

            var pathToReturn = await _userService.ChangeUserPhotoAsync(userId, photo);

            return StatusCode(StatusCodes.Status201Created, pathToReturn);
        }


        // api/users/5
        [AuthorizeRoles(Role.Manager)]
        [HttpDelete("{userId}")]
        [Description("Delete user by id")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult> DeleteUserAsync(int userId)
        {
            await _userService.DeleteUserAsync(userId);
            return NoContent();
        }

        // api/users/5/role/5
        [AuthorizeRoles()]
        [HttpPost("{userId}/role/{role}")]
        [Description("Add new role to user")]
        [ProducesResponseType(typeof(int), StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult> AddRoleToUserAsync(int userId, Role role)
        {
            await _userService.AddUserRoleAsync(userId, (int)role);
            return NoContent();
        }

        // api/users/5/role/5
        [AuthorizeRoles()]
        [HttpDelete("{userId}/role/{role}")]
        [Description("Delete role from user")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult> DeleteRoleFromUserAsync(int userId, Role role)
        {
            await _userService.DeleteUserRoleAsync(userId, (int)role);
            return NoContent();
        }
    }
}