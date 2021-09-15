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
            var addedUserId = _userRepository.AddUser(dto);


            if (dto.Roles == null || dto.Roles.Count == 0)
            {
                _userRepository.AddUserRole(addedUserId, Role.Student);
                return _userRepository.GetUserById(addedUserId);
            }

            if (dto.Roles[0] != Role.Student || dto.Roles.Count > 1)
            {
                if (userInfo.IsAdmin())
                {
                    foreach (var role in dto.Roles)
                    {
                        _userRepository.AddUserRole(addedUserId, role);
                    }
                }
                else
                {
                    throw new AuthorizationException(string.Format(
                        ServiceMessages.AdminCanAddRolesToUserMessage, nameof(Role.Admin)));
                }
            }
            
            return _userRepository.GetUserById(addedUserId);
        }

        public UserDto GetUserById(int id) => _userValidationHelper.GetUserByIdAndThrowIfNotFound(id);

        public UserDto GetUserByEmail(string email)
        {
            var user = _userRepository.GetUserByEmail(email);
            if (user == default)
                throw new EntityNotFoundException(string.Format(
                    ServiceMessages.EntityWithEmailNotFoundMessage, nameof(user), email));

            return user;
        }

        public List<UserDto> GetAllUsers() => _userRepository.GetAllUsers();

        public UserDto UpdateUser(UserDto dto)
        {
            _userValidationHelper.GetUserByIdAndThrowIfNotFound(dto.Id);
            _userRepository.UpdateUser(dto);
            return _userRepository.GetUserById(dto.Id);
        }

        public void DeleteUser(int id)
        {
            _userValidationHelper.GetUserByIdAndThrowIfNotFound(id);
            _userRepository.DeleteUser(id);
        }

        public void AddUserRole(int userId, Role roleId)
        {
            _userValidationHelper.GetUserByIdAndThrowIfNotFound(userId);
            _userRepository.AddUserRole(userId, roleId);
        }

        public void DeleteUserRole(int userId, Role roleId)
        {
            _userValidationHelper.GetUserByIdAndThrowIfNotFound(userId);
            _userRepository.DeleteUserRole(userId, roleId);
        }
    }
}