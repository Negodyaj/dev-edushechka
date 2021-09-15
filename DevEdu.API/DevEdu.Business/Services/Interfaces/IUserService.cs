using DevEdu.DAL.Models;
using System.Collections.Generic;
using DevEdu.Business.IdentityInfo;

namespace DevEdu.Business.Services
{
    public interface IUserService
    {
        UserDto AddUser(UserDto dto, UserIdentityInfo userInfo);
        void AddUserRole(int userId, int roleId, UserIdentityInfo userInfo);
        void DeleteUser(int id, UserIdentityInfo userInfo);
        void DeleteUserRole(int userId, int roleId, UserIdentityInfo userInfo);
        UserDto GetUserById(int id, UserIdentityInfo userInfo);
        public UserDto GetUserByEmail(string email);
        List<UserDto> GetAllUsers(UserIdentityInfo userInfo);
        UserDto UpdateUser(UserDto dto, UserIdentityInfo userInfo);
    }
}