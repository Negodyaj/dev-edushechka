using DevEdu.DAL.Models;
using System.Collections.Generic;

namespace DevEdu.DAL.Repositories
{
    public interface IUserRepository
    {
        int AddUser(UserDto user);
        void AddUserRole(int userId, int roleId);
        void DeleteUser(int id);
        void DeleteUserRole(int userId, int roleId);
        UserDto GetUserById(int id);
        public UserDto GetUserByEmail(string email);
        List<UserDto> GetAllUsers();
        void UpdateUser(UserDto user);
        List<UserDto> GetUsersByGroupIdAndRole(int GroupId, int Role);
    }
}