using AutoMapper;
using DevEdu.API.Models.InputModels;
using DevEdu.DAL.Models;
using DevEdu.DAL.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace DevEdu.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        UserRepository user = new UserRepository();
        public UserController() { }

        // api/user
        [HttpPost]
        public int AddUser([FromBody] UserInsertInputModel model)
        {
            return user.AddUser(MapUserModelToDto(model));
        }

        // api/user/userId
        [HttpPut("{userId}")]
        public int UpdateUserById(int userId, [FromBody] UserUpdateInputModel model)
        {
            return user.UpdateUser(userId, MapUserModelToDto(model));
        }

        // api/user/{userId}
        [HttpGet("{userId}")]
        public string GetUserById(int userId)
        {
            return userId.ToString();
        }

        // api/user
        [HttpGet]
        public string GetAllUsers()
        {
            return "here's for you all Users";
        }

        // api/user/{userId}
        [HttpDelete("{userId}")]
        public int DeleteUser(int userId)
        {
            return user.DeleteUser(userId);
        }

        // api/user/{userId}/role/{roleId}
        [HttpPost("{userId}/role/{roleId}")]
        public int AddRoleToUser(int userId, int roleId)
        {
            return 42;
        }

        private UserDto MapUserModelToDto(UserInsertInputModel model)
        {
            Mapper mapper = new Mapper(new MapperConfiguration(
                cfg => cfg.CreateMap<UserInsertInputModel, UserDto>()));
            return mapper.Map<UserDto>(model);
        }
        private UserDto MapUserModelToDto(UserUpdateInputModel model)
        {
            Mapper mapper = new Mapper(new MapperConfiguration(
                cfg => cfg.CreateMap<UserUpdateInputModel, UserDto>()));
            return mapper.Map<UserDto>(model);
        }
    }
}
