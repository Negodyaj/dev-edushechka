using DevEdu.DAL.Models;
using DevEdu.DAL.Repositories;
using System.Collections.Generic;
using DevEdu.DAL.Enums;

namespace DevEdu.Business.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public int AddUser(UserDto dto)
        {
            if (dto.Roles.Count == 0)
                dto.Roles.Add(Role.Student);

            var addedUserId = _userRepository.AddUser(dto);

            foreach (var role in dto.Roles)
                {
                    AddUserRole(addedUserId, (int)role);
                }
            
            return addedUserId;
        }

        public UserDto SelectUserById(int id) => _userRepository.SelectUserById(id);

        public UserDto SelectUserByEmail(string email) => _userRepository.SelectUserByEmail(email);

        public List<UserDto> SelectUsers() => _userRepository.SelectUsers();

        public UserDto UpdateUser(UserDto dto)
        {
            _userRepository.UpdateUser(dto);
            return _userRepository.SelectUserById(dto.Id);
        }

        public void DeleteUser(int id) => _userRepository.DeleteUser(id);

        public void AddUserRole(int userId, int roleId) => _userRepository.AddUserRole(userId, roleId);

        public void DeleteUserRole(int userId, int roleId) => _userRepository.DeleteUserRole(userId, roleId);
    }
}