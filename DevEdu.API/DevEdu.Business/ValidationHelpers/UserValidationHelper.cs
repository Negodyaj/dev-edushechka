using DevEdu.Business.Constants;
using DevEdu.Business.Exceptions;
using DevEdu.DAL.Enums;
using DevEdu.DAL.Repositories;
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

        public void CheckUserExistence(int userId)
        {
            var user = _userRepository.SelectUserById(userId);
            if (user == default)
                throw new EntityNotFoundException(string.Format(ServiceMessages.EntityNotFoundMessage, nameof(user), userId));
        }

        public void CheckAuthorizationUserToGroup(int groupId, int userId, Role role)
        {
            var usersInGroup = _userRepository.GetUsersByGroupIdAndRole(groupId, (int)role);
            if (usersInGroup == default || usersInGroup.FirstOrDefault(u => u.Id == userId) == default)
            {
                throw new AuthorizationException(string.Format(ServiceMessages.UserWithRoleDoesntAuthorizeToGroup, userId, groupId, role.ToString()));
            }
        }
    }
}