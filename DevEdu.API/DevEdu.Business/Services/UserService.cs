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
            return addedUserId;
        }

        public UserDto SelectUserById(int id)
        {
            _userValidationHelper.ChekIdDoesNotLessThenMinimum(id);
            var user = _userValidationHelper.GetUserDtoByIdAndCheckUserExistence(id);
            return user;
        }

        public UserDto SelectUserByEmail(string email)
        {
            var user = _userRepository.SelectUserByEmail(email);
            if (user == default)
                throw new EntityNotFoundException($"{nameof(user)} with email = {email} was not found");

            return user;
        }

        public List<UserDto> SelectUsers()
        {
            var list = _userRepository.SelectUsers();
            if (list == default || list.Count == 0)
                throw new Exception($"{nameof(list)} is empty");

            return list;
        }

        public UserDto UpdateUser(UserDto dto)
        {
            _userValidationHelper.GetUserDtoByIdAndCheckUserExistence(dto.Id);

            _userRepository.UpdateUser(dto);
            var user = _userRepository.SelectUserById(dto.Id);
            return user;
        }

        public void DeleteUser(int id)
        {
            _userValidationHelper.ChekIdDoesNotLessThenMinimum(id);
            _userValidationHelper.GetUserDtoByIdAndCheckUserExistence(id);

            _userRepository.DeleteUser(id);
        }

        public void AddUserRole(int userId, int roleId)
        {
            _userValidationHelper.CheckUserIdAndRoleIdDoesNotLessThanMinimum(userId, roleId);
            _userValidationHelper.ChekRoleExistence(roleId);

            _userRepository.AddUserRole(userId, roleId);
        }

        public void DeleteUserRole(int userId, int roleId)
        {
            _userValidationHelper.CheckUserIdAndRoleIdDoesNotLessThanMinimum(userId, roleId);
            _userValidationHelper.GetUserDtoByIdAndCheckUserExistence(userId);
            _userValidationHelper.ChekRoleExistence(roleId);

            _userRepository.DeleteUserRole(userId, roleId);
        }
    }
}