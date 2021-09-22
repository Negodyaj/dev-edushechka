using DevEdu.Business.Constants;
using DevEdu.Business.Exceptions;
using DevEdu.Business.ValidationHelpers;
using DevEdu.DAL.Enums;
using DevEdu.DAL.Models;
using DevEdu.DAL.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;
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

        public async Task<UserDto> AddUserAsync(UserDto dto, UserIdentityInfo userIdentity)
        {
            if (dto.Roles == null || dto.Roles.Count == 0)
                dto.Roles = new List<Role> { Role.Student };

            var addedUserId = await _userRepository.AddUserAsync(dto);

            if (userIdentity.IsAdmin())
            {
                foreach (var role in dto.Roles)
                {
                    await _userRepository.AddUserRoleAsync(addedUserId, (int)role);
                }
            }
            else
            {
                await _userRepository.AddUserRoleAsync(addedUserId, (int)Role.Student);
            }

            var response = await _userRepository.GetUserByIdAsync(addedUserId);

            return response;
        }

        public async Task<UserDto> GetUserByIdAsync(int id)
        {
            var user = await _userValidationHelper.GetUserByIdAndThrowIfNotFoundAsync(id);

            return user;
        }

        public async Task<UserDto> GetUserByEmailAsync(string email)
        {
            var user = await _userRepository.GetUserByEmailAsync(email);

            if (user == default)
                throw new EntityNotFoundException(string.Format(ServiceMessages.EntityWithEmailNotFoundMessage, nameof(user), email));

            return user;
        }

        public async Task<List<UserDto>> GetAllUsersAsync()
        {
            var list = await _userRepository.GetAllUsersAsync();
            return list;
        }

        public async Task<UserDto> UpdateUserAsync(UserDto dto)
        {
            await _userValidationHelper.GetUserByIdAndThrowIfNotFoundAsync(dto.Id);
            await _userRepository.UpdateUserAsync(dto);

            var user = await _userRepository.GetUserByIdAsync(dto.Id);

            return user;
        }

        public async Task DeleteUserAsync(int id)
        {
            await _userValidationHelper.GetUserByIdAndThrowIfNotFoundAsync(id);
            await _userRepository.DeleteUserAsync(id);
        }

        public async Task AddUserRoleAsync(int userId, Role roleId)
        {
            await _userValidationHelper.GetUserByIdAndThrowIfNotFoundAsync(userId);
            await _userRepository.AddUserRoleAsync(userId, (int)roleId);
        }

        public async Task DeleteUserRoleAsync(int userId, Role roleId)
        {
            await _userValidationHelper.GetUserByIdAndThrowIfNotFoundAsync(userId);
            await _userRepository.DeleteUserRoleAsync(userId, (int)roleId);
        }
    }
}