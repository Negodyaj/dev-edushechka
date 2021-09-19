using DevEdu.DAL.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DevEdu.DAL.Repositories
{
    public interface IUserRepository
    {
        Task<int> AddUser(UserDto user);
        Task AddUserRole(int userId, int roleId);
        Task DeleteUser(int id);
        Task DeleteUserRole(int userId, int roleId);
        Task<UserDto> GetUserById(int id);
        Task<UserDto> GetUserByEmail(string email);
        Task<List<UserDto>> GetAllUsers();
        Task UpdateUser(UserDto user);
        Task<List<UserDto>> GetUsersByGroupIdAndRole(int groupId, int role);
        Task<List<UserDto>> GetUsersByGroupIdAndRoleAsync(int groupId, int role);
    }
}