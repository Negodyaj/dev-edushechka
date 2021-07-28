using DevEdu.Business.Services;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using DevEdu.API.Models.InputModels;
using DevEdu.DAL.Models;
using Microsoft.AspNetCore.Http;

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

        [HttpPost("/register")]
        [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
        public int Register([FromBody] UserInsertInputModel model)
        {
            var dto = _mapper.Map<UserDto>(model);
            dto.Password = _authService.HashPassword(dto.Password);
            var addedUser = _userService.AddUser(dto);
            return addedUser;
        }

        [HttpPost("/sign-in")]
        public string SignIn(UserSignInputModel model)
        {
            var dto = _mapper.Map<UserDto>(model);
            return _authService.SignIn(dto);
        }
    }
}