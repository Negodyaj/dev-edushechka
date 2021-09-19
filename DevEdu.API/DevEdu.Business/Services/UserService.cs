using DevEdu.Business.Constants;
using DevEdu.Business.Exceptions;
using DevEdu.Business.ValidationHelpers;
using DevEdu.DAL.Enums;
using DevEdu.DAL.Models;
using DevEdu.DAL.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

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

        public async Task<UserDto> AddUser(UserDto dto)
        {
            if (dto.Roles == null || dto.Roles.Count == 0)
                dto.Roles = new List<Role> { Role.Student };

            var addedUserId = await _userRepository.AddUser(dto);

            foreach (var role in dto.Roles)
            {
                await _userRepository.AddUserRole(addedUserId, (int)role);
            }

            var response = await _userRepository.GetUserById(addedUserId);

            return response;
        }

        public async Task<UserDto> GetUserById(int id)
        {
            var user = await _userValidationHelper.GetUserByIdAndThrowIfNotFound(id);

            return user;
        }

        public async Task<UserDto> GetUserByEmail(string email)
        {
            var user = await _userRepository.GetUserByEmail(email);

            if (user == default)
                throw new EntityNotFoundException(string.Format(ServiceMessages.EntityWithEmailNotFoundMessage, nameof(user), email));

            return user;
        }

        public async Task<List<UserDto>> GetAllUsers()
        {
            var list = await _userRepository.GetAllUsers();

            return list;
        }

        public async Task<UserDto> UpdateUser(UserDto dto)
        {
            await _userValidationHelper.GetUserByIdAndThrowIfNotFound(dto.Id);
            await _userRepository.UpdateUser(dto);

            var user = await _userRepository.GetUserById(dto.Id);

            return user;
        }

        public async Task DeleteUser(int id)
        {
            await _userValidationHelper.GetUserByIdAndThrowIfNotFound(id);
            await _userRepository.DeleteUser(id);
        }

        public async Task AddUserRole(int userId, int roleId)
        {
            await _userValidationHelper.GetUserByIdAndThrowIfNotFound(userId);
            await _userRepository.AddUserRole(userId, roleId);
        }

        public async Task DeleteUserRole(int userId, int roleId)
        {
            await _userValidationHelper.GetUserByIdAndThrowIfNotFound(userId);
            await _userRepository.DeleteUserRole(userId, roleId);
        }
    }
}