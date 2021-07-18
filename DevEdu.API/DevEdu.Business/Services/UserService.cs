using DevEdu.DAL.Models;
using DevEdu.DAL.Repositories;
using System.Collections.Generic;

namespace DevEdu.Business.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public int AddUser(UserDto dto) => _userRepository.AddUser(dto);

        public UserDto SelectUserById(int id) => _userRepository.SelectUserById(id);

        public List<UserDto> SelectUsers() => _userRepository.SelectUsers();

        public List<UserDto> SelectUsersWithPasswords() => _userRepository.SelectUsersWithPasswords();

        public void UpdateUser(UserDto dto) => _userRepository.UpdateUser(dto);

        public void DeleteUser(int id) => _userRepository.DeleteUser(id);

        public int AddUserRole(int userId, int roleId) => _userRepository.AddUserRole(userId, roleId);

        public void DeleteUserRole(int userId, int roleId) => _userRepository.DeleteUserRole(userId, roleId);
    }
}