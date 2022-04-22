using DevEdu.Business.IdentityInfo;
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
        Task ChangePasswordUserAsync(UserDto dto);
        Task<string> ChangeUserPhotoAsync(int userId, IFormFile photo);
        Task DeleteUserAsync(int id);
        Task DeleteUserRoleAsync(int userId, int roleId);
        Task<List<UserDto>> GetAllUsersAsync();
        Task<UserDto> GetUserByEmailAsync(string email);
        Task<UserDto> GetUserByIdAsync(int id, UserIdentityInfo userInfo=null);
        Task<(UserDto, List<GroupDto>)> GetUserByTokenAsync(UserIdentityInfo userInfo = null);
        Task<UserDto> UpdateUserAsync(UserDto dto, UserIdentityInfo userInfo);
    }
}