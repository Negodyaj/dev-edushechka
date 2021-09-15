﻿using AutoMapper;
using DevEdu.API.Configuration.ExceptionResponses;
using DevEdu.API.Models;
using DevEdu.Business.Services;
using DevEdu.DAL.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using DevEdu.API.Common;
using DevEdu.API.Extensions;
using DevEdu.DAL.Enums;

namespace DevEdu.API.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthenticationController : Controller
    {
        private readonly IAuthenticationService _authService;
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public AuthenticationController(
            IUserService userService,
            IMapper mapper,
            IAuthenticationService authService)
        {
            _authService = authService;
            _userService = userService;
            _mapper = mapper;
        }

        [AuthorizeRoles(Role.Manager)]
        [HttpPost("/register")]
        [ProducesResponseType(typeof(UserFullInfoOutPutModel), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ValidationExceptionResponse), StatusCodes.Status422UnprocessableEntity)]
        public ActionResult<UserFullInfoOutPutModel> Register([FromBody] UserInsertInputModel model)
        {
            var userInfo = this.GetUserIdAndRoles();
            var dto = _mapper.Map<UserDto>(model);
            dto.Password = _authService.HashPassword(dto.Password);
            var addedUser = _mapper.Map<UserFullInfoOutPutModel>(_userService.AddUser(dto, userInfo));
            return Created(new Uri($"api/User/{addedUser.Id}", UriKind.Relative), addedUser);
        }

        [HttpPost("/sign-in")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ValidationExceptionResponse), StatusCodes.Status422UnprocessableEntity)]
        public string SignIn(UserSignInputModel model)
        {
            var dto = _mapper.Map<UserDto>(model);
            return _authService.SignIn(dto);
        }
    }
}