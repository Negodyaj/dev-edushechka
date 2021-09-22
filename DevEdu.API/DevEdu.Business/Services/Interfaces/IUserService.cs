using DevEdu.DAL.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using DevEdu.Business.IdentityInfo;
using DevEdu.DAL.Enums;

namespace DevEdu.Business.Services
{
    public interface IUserService
    {
        Task<UserDto> AddUserAsync(UserDto dto, UserIdentityInfo userIdentity);
        Task AddUserRoleAsync(int userId, Role role);
        Task DeleteUserAsync(int id);
        Task DeleteUserRoleAsync(int userId, Role role);
        Task<UserDto> GetUserByIdAsync(int id);
        Task<UserDto> GetUserByEmailAsync(string email);
        Task<List<UserDto>> GetAllUsersAsync();
        Task<UserDto> UpdateUserAsync(UserDto dto);
    }
}