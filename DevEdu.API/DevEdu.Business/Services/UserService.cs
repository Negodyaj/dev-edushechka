using DevEdu.Business.Constants;
using DevEdu.Business.Exceptions;
using DevEdu.Business.IdentityInfo;
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

        public async Task<UserDto> AddUserAsync(UserDto dto)
        {
            var userInDb = await _userRepository.GetUserByEmailAsync(dto.Email);
            if (userInDb != null)
            {
                throw new NotUniqueException(nameof(UserDto.Email));
            }

            var addedUserId = await _userRepository.AddUserAsync(dto);

            await _userRepository.AddUserRoleAsync(addedUserId, (int)Role.Student);

            var response = await _userRepository.GetUserByIdAsync(addedUserId);

            return response;
        }

        public async Task<UserDto> GetUserByIdAsync(int getInfoUserid, UserIdentityInfo userInfo=null)
        {
            if (userInfo != null)
                _userValidationHelper.CheckAccessChangeDataForUserAsync(getInfoUserid, userInfo.UserId, userInfo.Roles);

            var user = await _userValidationHelper.GetUserByIdAndThrowIfNotFoundAsync(getInfoUserid);

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

        public async Task<UserDto> UpdateUserAsync(UserDto dto, UserIdentityInfo userInfo=null)
        {
            if (userInfo != null)
                _userValidationHelper.CheckAccessChangeDataForUserAsync(dto.Id, userInfo.UserId, userInfo.Roles);

            await _userValidationHelper.GetUserByIdAndThrowIfNotFoundAsync(dto.Id);
            await _userRepository.UpdateUserAsync(dto);

            var user = await _userRepository.GetUserByIdAsync(dto.Id);

            return user;
        }

        public async Task ChangePasswordUserAsync(UserDto dto)
        {
            await _userRepository.UpdateUserPasswordAsync(dto);
        }

        public async Task DeleteUserAsync(int id)
        {
            await _userValidationHelper.GetUserByIdAndThrowIfNotFoundAsync(id);
            await _userRepository.DeleteUserAsync(id);
        }

        public async Task AddUserRoleAsync(int userId, int roleId)
        {
            await _userValidationHelper.GetUserByIdAndThrowIfNotFoundAsync(userId);
            await _userRepository.AddUserRoleAsync(userId, roleId);
        }

        public async Task DeleteUserRoleAsync(int userId, int roleId)
        {
            await _userValidationHelper.GetUserByIdAndThrowIfNotFoundAsync(userId);
            await _userRepository.DeleteUserRoleAsync(userId, roleId);
        }
    }
}