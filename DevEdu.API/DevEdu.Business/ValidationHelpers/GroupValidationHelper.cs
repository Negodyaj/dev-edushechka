using DevEdu.Business.Constants;
using DevEdu.Business.Exceptions;
using DevEdu.DAL.Enums;
using DevEdu.DAL.Repositories;

namespace DevEdu.Business.ValidationHelpers
{
    public class GroupValidationHelper : IGroupValidationHelper
    {
        private readonly IGroupRepository _groupRepository;

        public GroupValidationHelper(IGroupRepository groupRepository)
        {
            _groupRepository = groupRepository;
        }

        public void CheckGroupExistence(int groupId)
        {
            //var group = _groupRepository.GetGroup(groupId);
            //if (group == default)
            //    throw new EntityNotFoundException(string.Format(ServiceMessages.EntityNotFoundMessage, nameof(group), groupId));
        }
        public void CheckUserBelongToGroup(int groupId, int userId, Role role)
        {
            var userGroupId = _groupRepository.GetUser_GroupByUserIdAndUserRoleAndGroupId(userId, role, groupId);
            if (userGroupId == default)
            {
                throw new ValidationException(string.Format(ServiceMessages.UserDoesntBelongToGroup, role, userId, groupId));
            }
        }
        public void CheckTeacherBelongToGroup(int groupId, int teacherId, Role role)
        {
            var userGroupId = _groupRepository.GetUser_GroupByUserIdAndUserRoleAndGroupId(teacherId, role, groupId);
            if (userGroupId == default)
            {
                throw new AuthorizationException(string.Format(ServiceMessages.UserDoesntBelongToGroup, role, teacherId, groupId));
            }
        }
    }
}