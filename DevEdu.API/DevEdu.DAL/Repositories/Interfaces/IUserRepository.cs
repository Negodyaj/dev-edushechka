using DevEdu.DAL.Models;
using System.Collections.Generic;

namespace DevEdu.DAL.Repositories
{
    public interface IUserRepository
    {
        int AddUser(UserDto user);
        int AddUserRole(int userId, int roleId);
        void DeleteUser(int id);
        void DeleteUserRole(int userId, int roleId);
        UserDto SelectUserById(int id);
        List<UserDto> SelectUsers();
        public List<UserDto> SelectUsersWithPasswords();
        void UpdateUser(UserDto user);
    }
}