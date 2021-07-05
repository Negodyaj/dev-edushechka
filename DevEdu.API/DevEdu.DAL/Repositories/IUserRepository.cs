using DevEdu.DAL.Models;
using System.Collections.Generic;

namespace DevEdu.DAL.Repositories
{
    public interface IUserRepository
    {
        int AddUser(UserDto user);
        void DeleteUser(int id);
        UserDto SelectUserById(int id);
        List<UserDto> SelectUsers();
        void UpdateUser(UserDto user);
    }
}