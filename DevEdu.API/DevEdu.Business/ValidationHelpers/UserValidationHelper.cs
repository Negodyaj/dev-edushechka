using DevEdu.Business.Constants;
using DevEdu.Business.Exceptions;
using DevEdu.DAL.Enums;
using DevEdu.DAL.Models;
using DevEdu.DAL.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DevEdu.Business.ValidationHelpers
{
    public class UserValidationHelper : IUserValidationHelper
    {
        private readonly IUserRepository _userRepository;

        public UserValidationHelper(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<UserDto> GetUserByIdAndThrowIfNotFound(int userId)
        {
            var user = await _userRepository.GetUserByIdAsync(userId);

            if (user == default)
                throw new EntityNotFoundException(string.Format(ServiceMessages.EntityNotFoundMessage, nameof(user), userId));

            return user;
        }

        public async Task CheckUserBelongToGroup(int groupId, int userId, Role role)
        {
            var usersInGroup = await _userRepository.GetUsersByGroupIdAndRoleAsync(groupId, (int)role);

            if (usersInGroup == default ||
                usersInGroup.FirstOrDefault(u => u.Id == userId) == default)
            {
                throw new ValidationException(nameof(StudentRatingDto.User),
                    string.Format(ServiceMessages.UserWithRoleDoesntBelongToGroup, role.ToString(), userId, groupId));
            }
        }

        public async Task CheckUserBelongToGroup(int groupId, int userId, List<Role> roles)
        {
            var checkResult = false;

            foreach (var role in roles)
            {
                var usersInGroup = await _userRepository.GetUsersByGroupIdAndRoleAsync(groupId, (int)role);

                if (usersInGroup != default &&
                    usersInGroup.FirstOrDefault(u => u.Id == userId) != default)
                {
                    checkResult = true;
                }
            }

            if (!checkResult)
            {
                throw new ValidationException(nameof(userId), string.Format(ServiceMessages.UserDoesntBelongToGroup, userId, groupId));
            }
        }

        public async Task CheckAuthorizationUserToGroup(int groupId, int userId, Role role)
        {
            var usersInGroup = await _userRepository.GetUsersByGroupIdAndRoleAsync(groupId, (int)role);

            if (usersInGroup == default ||
                usersInGroup.FirstOrDefault(u => u.Id == userId) == default)
            {
                throw new AuthorizationException(string.Format(ServiceMessages.UserWithRoleDoesntAuthorizeToGroup, userId, groupId, role.ToString()));
            }
        }
    }
}