using DevEdu.DAL.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DevEdu.Business.Services
{
    public interface IUserService
    {
       Task<UserDto> AddUser(UserDto dto);
        Task AddUserRole(int userId, int roleId);
        Task DeleteUser(int id);
        Task DeleteUserRole(int userId, int roleId);
        Task<UserDto> GetUserById(int id);
        Task<UserDto> GetUserByEmail(string email);
        Task<List<UserDto>> GetAllUsers();
        Task<UserDto> UpdateUser(UserDto dto);
    }
}