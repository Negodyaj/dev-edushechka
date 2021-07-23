using DevEdu.Business.Exceptions;
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

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public int AddUser(UserDto dto)
        {
            var addedUserId = _userRepository.AddUser(dto);

            if (dto.Roles == null || dto.Roles.Count == 0)
            {
                return addedUserId;
            }

            foreach (var role in dto.Roles)
            {
                AddUserRole(addedUserId, (int)role);
            }
            return addedUserId;
        }

        public UserDto SelectUserById(int id)
        {
            var user = _userRepository.SelectUserById(id);
            if (user == default)
                throw new EntityNotFoundException($"user with id = {id} was not found");

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
            var user = _userRepository.SelectUserById(dto.Id);
            if (user == default)
                throw new EntityNotFoundException($"{nameof(user)} with id = {user.Id} was not found");

            _userRepository.UpdateUser(dto);
            return _userRepository.SelectUserById(dto.Id);
        }

        public void DeleteUser(int id)
        {
            var user = _userRepository.SelectUserById(id);
            if (user == default)
                throw new EntityNotFoundException($"{nameof(user)} with id = {id} was not found");

            _userRepository.DeleteUser(id);
        }

        public void AddUserRole(int userId, int roleId)
        {
            var user = _userRepository.SelectUserById(userId);
            if (user == default)
                throw new EntityNotFoundException($"{nameof(user)} with id = {userId} was not found");

            var role = Enum.GetName(typeof(Role), roleId);
            if (role == default)
                throw new EntityNotFoundException($"{nameof(role)} with id = {roleId} was not found");

            _userRepository.AddUserRole(userId, roleId);
        }

        public void DeleteUserRole(int userId, int roleId)
        {
            var user = _userRepository.SelectUserById(userId);
            if (user == default)
                throw new EntityNotFoundException($"{nameof(user)} with id = {userId} was not found");

            var role = Enum.GetName(typeof(Role), roleId);
            if (role == default)
                throw new EntityNotFoundException($"{nameof(role)} with id = {roleId} was not found");

            _userRepository.DeleteUserRole(userId, roleId);
        }
    }
}