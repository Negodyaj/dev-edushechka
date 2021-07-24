using DevEdu.Business.Constants;
using DevEdu.Business.Exceptions;
using DevEdu.DAL.Enums;
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

        public void CheckUserExistence(int userId)
        {
            var user = _userRepository.SelectUserById(userId);
            if (user == default)
                throw new EntityNotFoundException(string.Format(ServiceMessages.EntityNotFoundMessage, nameof(user), userId));
        }

        public void ChekRoleExistence(int roleId)
        {
            var role = Enum.GetName(typeof(Role), roleId);
            if (role == default)
                throw new EntityNotFoundException(string.Format(ServiceMessages.EntityNotFoundMessage, nameof(role), roleId));
        }
    }
}