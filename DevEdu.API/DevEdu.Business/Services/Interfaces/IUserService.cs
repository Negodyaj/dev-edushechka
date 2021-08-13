using DevEdu.DAL.Models;
using System.Collections.Generic;

namespace DevEdu.Business.Services
{
    public interface IUserService
    {
        UserDto AddUser(UserDto dto);
        void AddUserRole(int userId, int roleId);
        void DeleteUser(int id);
        void DeleteUserRole(int userId, int roleId);
        UserDto GetUserById(int id);
        public UserDto GetUserByEmail(string email);
        List<UserDto> GetAllUsers();
        UserDto UpdateUser(UserDto dto);
    }
}