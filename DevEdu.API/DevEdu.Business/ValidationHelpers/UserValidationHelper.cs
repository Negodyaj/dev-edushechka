﻿using DevEdu.Business.Constants;
using DevEdu.Business.Exceptions;
using DevEdu.DAL.Enums;
using DevEdu.DAL.Models;
using DevEdu.DAL.Repositories;
using System.Collections.Generic;
using System.Linq;

namespace DevEdu.Business.ValidationHelpers
{
    public class UserValidationHelper : IUserValidationHelper
    {
        private readonly IUserRepository _userRepository;

        public UserValidationHelper(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public UserDto GetUserByIdAndThrowIfNotFound(int userId)
        {
            var user = _userRepository.GetUserById(userId);
            if (user == default)
                throw new EntityNotFoundException(string.Format(ServiceMessages.EntityWithIdNotFoundMessage, nameof(user), userId));

            return user;
        }

        public void CheckUserBelongToGroup(int groupId, int userId, Role role)
        {
            var usersInGroup = _userRepository.GetUsersByGroupIdAndRole(groupId, (int)role);

            if (usersInGroup == default || usersInGroup.FirstOrDefault(u => u.Id == userId) == default)
            {
                throw new ValidationException(nameof(StudentRatingDto.User), string.Format(ServiceMessages.UserWithRoleDoesntBelongToGroupMessage, role.ToString(), userId, groupId));
            }
        }

        public void CheckUserBelongToGroup(int groupId, int userId, List<Role> roles)
        {
            var checkResult = false;

            foreach (var role in roles)
            {
                var usersInGroup = _userRepository.GetUsersByGroupIdAndRole(groupId, (int)role);

                if (usersInGroup != default && usersInGroup.FirstOrDefault(u => u.Id == userId) != default)
                {
                    checkResult = true;
                }
            }

            if (!checkResult)
            {
                throw new ValidationException(nameof(userId), string.Format(ServiceMessages.UserDoesntBelongToGroupMessage, userId, groupId));
            }
        }

        public void CheckAuthorizationUserToGroup(int groupId, int userId, Role role)
        {
            var usersInGroup = _userRepository.GetUsersByGroupIdAndRole(groupId, (int)role);

            if (usersInGroup == default || usersInGroup.FirstOrDefault(u => u.Id == userId) == default)
            {
                throw new AuthorizationException(string.Format(ServiceMessages.UserWithRoleDoesntAuthorizeToGroupMessage, userId, groupId, role.ToString()));
            }
        }
    }
}