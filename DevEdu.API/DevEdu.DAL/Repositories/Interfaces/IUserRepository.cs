using DevEdu.DAL.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DevEdu.DAL.Repositories
{
    public interface IUserRepository
    {
        Task<int> AddUserAsync(UserDto user);
        Task AddUserRoleAsync(int userId, int roleId);
        Task DeleteUserAsync(int id);
        Task DeleteUserRoleAsync(int userId, int roleId);
        Task<UserDto> GetUserByIdAsync(int id);
        Task<UserDto> GetUserByEmailAsync(string email);
        Task<List<UserDto>> GetAllUsersAsync();
        Task UpdateUserAsync(UserDto user);
        Task<List<UserDto>> GetUsersByGroupIdAndRoleAsync(int groupId, int role);
        Task UpdateUserPasswordAsync(UserDto user);
        Task UpdateUserPhotoAsync(int id, string photo);

        //Task<List<UserDto>> GetUsersByGroupIdAndRoleAsync(int groupId, int role);
    }
}