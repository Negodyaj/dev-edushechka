using DevEdu.Business.Constants;
using DevEdu.Business.Exceptions;
using DevEdu.Business.ValidationHelpers;
using DevEdu.DAL.Enums;
using DevEdu.DAL.Models;
using DevEdu.DAL.Repositories;
using System.Collections.Generic;
using DevEdu.Business.IdentityInfo;

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

        public UserDto AddUser(UserDto dto, UserIdentityInfo userInfo)
        {
            if (dto.Roles == null || dto.Roles.Count == 0)
                dto.Roles = new List<Role> { Role.Student };

            var addedUserId = _userRepository.AddUser(dto);

            foreach (var role in dto.Roles)
            {
                _userRepository.AddUserRole(addedUserId, (int)role);
            }

            var response = _userRepository.GetUserById(addedUserId);
            return response;
        }

        public UserDto GetUserById(int id, UserIdentityInfo userInfo)
        {
            var user = _userValidationHelper.GetUserByIdAndThrowIfNotFound(id);
            return user;
        }

        public UserDto GetUserByEmail(string email)
        {
            var user = _userRepository.GetUserByEmail(email);
            if (user == default)
                throw new EntityNotFoundException(string.Format(ServiceMessages.EntityWithEmailNotFoundMessage, nameof(user), email));

            return user;
        }

        public List<UserDto> GetAllUsers(UserIdentityInfo userInfo)
        {
            var list = _userRepository.GetAllUsers();
            return list;
        }

        public UserDto UpdateUser(UserDto dto, UserIdentityInfo userInfo)
        {
            _userValidationHelper.GetUserByIdAndThrowIfNotFound(dto.Id);

            _userRepository.UpdateUser(dto);
            var user = _userRepository.GetUserById(dto.Id);
            return user;
        }

        public void DeleteUser(int id, UserIdentityInfo userInfo)
        {
            _userValidationHelper.GetUserByIdAndThrowIfNotFound(id);
            _userRepository.DeleteUser(id);
        }

        public void AddUserRole(int userId, int roleId, UserIdentityInfo userInfo)
        {
            _userValidationHelper.GetUserByIdAndThrowIfNotFound(userId);
            _userRepository.AddUserRole(userId, roleId);
        }

        public void DeleteUserRole(int userId, int roleId, UserIdentityInfo userInfo)
        {
            _userValidationHelper.GetUserByIdAndThrowIfNotFound(userId);
            _userRepository.DeleteUserRole(userId, roleId);
        }
    }
}