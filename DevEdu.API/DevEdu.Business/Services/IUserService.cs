using DevEdu.DAL.Models;
using System.Collections.Generic;

namespace DevEdu.Business.Servicies
{
    public interface IUserService
    {
        int AddUser(UserDto dto);
        void AddUserRole(int userId, int roleId);
        void DeleteUser(int id);
        void DeleteUserRole(int userId, int roleId);
        UserDto SelectUserById(int id);
        List<UserDto> SelectUsers();
        UserDto UpdateUser(UserDto dto);
    }
}