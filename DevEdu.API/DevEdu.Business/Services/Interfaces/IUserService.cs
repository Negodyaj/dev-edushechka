using DevEdu.DAL.Models;
using System.Collections.Generic;
using DevEdu.Business.IdentityInfo;
using DevEdu.DAL.Enums;

namespace DevEdu.Business.Services
{
    public interface IUserService
    {
        UserDto AddUser(UserDto dto, UserIdentityInfo userInfo);
        void AddUserRole(int userId, Role roleId);
        void DeleteUser(int id);
        void DeleteUserRole(int userId, Role roleId);
        UserDto GetUserById(int id);
        public UserDto GetUserByEmail(string email);
        List<UserDto> GetAllUsers();
        UserDto UpdateUser(UserDto dto);
    }
}