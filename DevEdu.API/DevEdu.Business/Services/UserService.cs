using DevEdu.Business.Constants;
using DevEdu.Business.Exceptions;
using DevEdu.Business.ValidationHelpers;
using DevEdu.DAL.Enums;
using DevEdu.DAL.Models;
using DevEdu.DAL.Repositories;
using System.Collections.Generic;

namespace DevEdu.Business.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IUserValidationHelper _userValidationHelper;

        public UserService(IUserRepository userRepository, IUserValidationHelper helper)
        {
            _userRepository = userRepository;
            _userValidationHelper = helper;
        }

        public UserDto AddUser(UserDto dto)
        {
            if (dto.Roles == null || dto.Roles.Count == 0)
                dto.Roles = new List<Role> { Role.Student };

            var addedUserId = _userRepository.AddUser(dto);

            foreach (var role in dto.Roles)
            {
                _userRepository.AddUserRole(addedUserId, (int)role);
            }

            var response = _userRepository.SelectUserById(addedUserId);
            return response;
        }

        public UserDto SelectUserById(int id)
        {
            var user = _userValidationHelper.GetUserByIdAndThrowIfNotFound(id);
            return user;
        }

        public UserDto SelectUserByEmail(string email)
        {
            var user = _userRepository.SelectUserByEmail(email);
            if (user == default)
                throw new EntityNotFoundException(string.Format(ServiceMessages.EntityWithEmailNotFoundMessage, nameof(user), email));

            return user;
        }

        public List<UserDto> SelectUsers()
        {
            var list = _userRepository.SelectUsers();
            return list;
        }

        public UserDto UpdateUser(UserDto dto)
        {
            _userValidationHelper.GetUserByIdAndThrowIfNotFound(dto.Id);

            _userRepository.UpdateUser(dto);
            var user = _userRepository.SelectUserById(dto.Id);
            return user;
        }

        public void DeleteUser(int id)
        {
            _userValidationHelper.GetUserByIdAndThrowIfNotFound(id);
            _userRepository.DeleteUser(id);
        }

        public void AddUserRole(int userId, int roleId)
        {
            _userValidationHelper.GetUserByIdAndThrowIfNotFound(userId);
            _userRepository.AddUserRole(userId, roleId);
        }

        public void DeleteUserRole(int userId, int roleId)
        {
            _userValidationHelper.GetUserByIdAndThrowIfNotFound(userId);
            _userRepository.DeleteUserRole(userId, roleId);
        }
    }
}