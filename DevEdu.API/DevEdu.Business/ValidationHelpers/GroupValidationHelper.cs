using System.Linq;
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
            var group = _groupRepository.GetGroup(groupId);
            if (group == default)
                throw new EntityNotFoundException(string.Format(ServiceMessages.EntityNotFoundMessage, nameof(group), groupId));
        }

        public void CheckUserInGroupExistence(int groupId, int userId)
        {
            var groupsByUser = _groupRepository.GetGroupsByUserId(userId);
            var group = _groupRepository.GetGroup(groupId);
            var result = groupsByUser.FirstOrDefault(gu => gu.Id == @group.Id);
            if (result == default)
                throw new AuthorizationException(string.Format(ServiceMessages.UserInGroupNotFoundMessage, userId, groupId));
        }
    }
}