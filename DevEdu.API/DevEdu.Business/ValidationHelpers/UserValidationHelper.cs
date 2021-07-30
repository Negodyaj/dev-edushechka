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

        public UserValidationHelper(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public UserDto GetUserByIdAndThrowIfNotFound(int userId)
        {
            var user = _userRepository.SelectUserById(userId);
            if (user == default)
                throw new EntityNotFoundException(string.Format(ServiceMessages.EntityWithIdNotFoundMessage, nameof(user), userId));

            return user;
        }
    }
}