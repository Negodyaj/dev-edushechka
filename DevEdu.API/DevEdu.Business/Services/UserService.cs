using DevEdu.Business.Constants;
using DevEdu.Business.Exceptions;
using DevEdu.Business.ValidationHelpers;
using DevEdu.DAL.Enums;
using DevEdu.DAL.Models;
using DevEdu.DAL.Repositories;
using System;
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

        public int AddUser(UserDto dto)
        {
            var addedUserId = _userRepository.AddUser(dto);

            if (dto.Roles == null || dto.Roles.Count == 0)
            {
                AddUserRole(addedUserId, ((int)Role.Student));
            }

            foreach (var role in dto.Roles)
            {
                AddUserRole(addedUserId, (int)role);
            }
            return addedUserId;
        }

        public UserDto SelectUserById(int id)
        {
            if (id < 0)
            {
                throw new Exception($"{nameof(id)} less then 0");
            }
            _userValidationHelper.CheckUserExistence(id);

            var user = _userRepository.SelectUserById(id);
            
            return user;
        }

        public UserDto SelectUserByEmail(string email)
        {
            var user = _userRepository.SelectUserByEmail(email);
            if (user == default)
                throw new EntityNotFoundException($"{nameof(user)} with email = {email} was not found");

            return user;
        }

        public List<UserDto> SelectUsers() => _userRepository.SelectUsers();

        public UserDto UpdateUser(UserDto dto)
        {
            _userValidationHelper.CheckUserExistence(dto.Id);

            _userRepository.UpdateUser(dto);
            return _userRepository.SelectUserById(dto.Id);
        }

        public void DeleteUser(int id)
        {
            _userValidationHelper.CheckUserExistence(id);

            _userRepository.DeleteUser(id);
        }

        public void AddUserRole(int userId, int roleId)
        {
            _userValidationHelper.CheckUserExistence(userId);
            _userValidationHelper.ChekRoleExistence(roleId);

            _userRepository.AddUserRole(userId, roleId);
        }

        public void DeleteUserRole(int userId, int roleId)
        {
            _userValidationHelper.CheckUserExistence(userId);
            _userValidationHelper.ChekRoleExistence(roleId);

            _userRepository.DeleteUserRole(userId, roleId);
        }
    }
}