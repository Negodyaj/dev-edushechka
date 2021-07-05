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
        UserRoleRepository _userRoleRepository;
        private readonly IMapper _mapper;

        public UserController(IMapper mapper)
        {
            _mapper = mapper;
            _userRepository = new UserRepository();
            _userRoleRepository = new UserRoleRepository();
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
        public void UpdateUserById([FromBody] UserUpdateInputModel model)
        {
            var dto = _mapper.Map<UserDto>(model);
            _userRepository.UpdateUser(dto);
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
            return _userRoleRepository.AddUserRole(userId, roleId);
        }

        // api/user/{userId}/role/{roleId}
        [HttpDelete("{userId}/role/{roleId}")]
        public void DeleteRoleFromUser(int userId, int roleId)
        {
            _userRoleRepository.DeleteUserRole(userId, roleId);
        }
    }
}
