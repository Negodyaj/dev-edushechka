using AutoMapper;
using DevEdu.API.Models.InputModels;
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
        UserRepository _userRepository;
        private readonly IMapper _mapper;

        public UserController(Mapper mapper) {
            _mapper = mapper;
            _userRepository = new UserRepository();
        }

        // api/user
        [HttpPost]
        public int AddUser([FromBody] UserInsertInputModel model)
        {
            var dto = _mapper.Map<UserDto>(model);
            return _userRepository.AddUser(dto);
        }

        // api/user/userId
        [HttpPut("{userId}")]
        public void UpdateUserById(int userId, [FromBody] UserUpdateInputModel model)
        {
            var dto = _mapper.Map<UserDto>(model);
            _userRepository.UpdateUser(userId, dto);
        }

        // api/user/{userId}
        [HttpGet("{userId}")]
        public UserDto GetUserById(int userId)
        {
            return _userRepository.SelectUserById(userId);
        }

        // api/user
        [HttpGet]
        public List<UserDto> GetAllUsers()
        {
            return _userRepository.SelectUsers();
        }

        // api/user/{userId}
        [HttpDelete("{userId}")]
        public void DeleteUser(int userId)
        {
            _userRepository.DeleteUser(userId);
        }

        // api/user/{userId}/role/{roleId}
        [HttpPost("{userId}/role/{roleId}")]
        public int AddRoleToUser(int userId, int roleId)
        {
            return 42;
        }
    }
}
