using DevEdu.Business.Constants;
using DevEdu.Business.Exceptions;
using DevEdu.DAL.Enums;
using DevEdu.DAL.Models;
using DevEdu.DAL.Repositories;
using System;

namespace DevEdu.Business.ValidationHelpers
{
    public class UserValidationHelper : IUserValidationHelper
    {
        private readonly IUserRepository _userRepository;
        private readonly int idMinimum = 1;

        public UserValidationHelper(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public UserDto GetUserByIdAndThrowIfNotFound(int userId)
        {
            var user = _userRepository.SelectUserById(userId);
            if (user == default)
                throw new EntityNotFoundException(string.Format(ServiceMessages.EntityNotFoundMessage, nameof(user), userId));

            return user;
        }

        public void ChekRoleExistence(int roleId)
        {
            var role = Enum.GetName(typeof(Role), roleId);
            if (role == default)
                throw new EntityNotFoundException(string.Format(ServiceMessages.EntityNotFoundMessage, nameof(role), roleId));
        }

        public void CheckUserIdAndRoleIdDoesNotLessThanMinimum(int userId, int roleId)
        {
            if (userId < idMinimum || roleId < idMinimum)
                throw new Exception(string.Format(ServiceMessages.MinimumAllowedValueMessage, nameof(userId), nameof(roleId), idMinimum));
        }

        public void ChekIdDoesNotLessThenMinimum(int id)
        {
            if (id < idMinimum)
                throw new Exception(string.Format(ServiceMessages.MinimumAllowedValueMessage, nameof(id), idMinimum));
        }
    }
}