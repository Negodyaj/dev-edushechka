using DevEdu.Business.IdentityInfo;
using DevEdu.DAL.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DevEdu.Business.Services
{
    public interface IUserService
    {
        Task<UserDto> AddUserAsync(UserDto dto);
        Task AddUserRoleAsync(int userId, int roleId);
        Task ChangePasswordUserAsync(UserDto dto);
        Task DeleteUserAsync(int id);
        Task DeleteUserRoleAsync(int userId, int roleId);
        Task<List<UserDto>> GetAllUsersAsync();
        Task<UserDto> GetUserByEmailAsync(string email);
        Task<UserDto> GetUserByIdAsync(int id, UserIdentityInfo userInfo);
        Task<UserDto> UpdateUserAsync(UserDto dto, UserIdentityInfo userInfo);
    }
}