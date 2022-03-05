using DevEdu.DAL.Models;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DevEdu.Business.Services
{
    public interface IUserService
    {
        Task<UserDto> AddUserAsync(UserDto dto);
        Task AddUserRoleAsync(int userId, int roleId);
        Task DeleteUserAsync(int id);
        Task DeleteUserRoleAsync(int userId, int roleId);
        Task<UserDto> GetUserByIdAsync(int id);
        Task<UserDto> GetUserByEmailAsync(string email);
        Task<List<UserDto>> GetAllUsersAsync();
        Task<UserDto> UpdateUserAsync(UserDto dto);
        Task ChangePasswordUserAsync(UserDto dto);
        Task<string> ChangeUserPhotoAsync(int userId, IFormFile photo);
    }
}