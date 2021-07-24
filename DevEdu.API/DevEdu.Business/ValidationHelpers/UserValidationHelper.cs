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

        public void CheckUserIdAndRoleIdDoesNotLessThanZero(int userId, int roleId)
        {
            if (userId < 0 || roleId < 0)
                throw new Exception($"{nameof(userId)} or {nameof(roleId)} less then 0");
        }

        public void ChekUserIdDoesNotLessThenZero(int id)
        {
            if (id < 0)
                throw new Exception($"{nameof(id)} less then 0");
        }
    }
}