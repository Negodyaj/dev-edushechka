using DevEdu.Business.Constants;
using DevEdu.Business.Exceptions;
using DevEdu.Business.IdentityInfo;
using DevEdu.DAL.Repositories;
using System.Threading.Tasks;

namespace DevEdu.Business.ValidationHelpers
{
    public class GroupValidationHelper : IGroupValidationHelper
    {
        private readonly IGroupRepository _groupRepository;

        public GroupValidationHelper(IGroupRepository groupRepository)
        {
            _groupRepository = groupRepository;
        }

        public async Task CheckGroupExistence(int groupId)
        {
            var group = await _groupRepository.GetGroup(groupId);
            if (group == default)
                throw new EntityNotFoundException(string.Format(ServiceMessages.EntityNotFoundMessage, nameof(group), groupId));
        }

        public void CheckAccessGetGroupMembers(int groupId, UserIdentityInfo userInfo)
        {
            throw new AuthorizationException(string.Format(ServiceMessages.AccessDeniedForGetGroupMembers, groupId));
        }

        public void CheckAccessGroup(UserIdentityInfo userInfo, int groupId)
        {
            throw new System.NotImplementedException();
        }

        public void CheckAccessGroupAndMaterial(UserIdentityInfo userInfo, int groupId, int materialId)
        {
            throw new System.NotImplementedException();
        }

        public void CheckAccessGroupAndLesson(UserIdentityInfo userInfo, int groupId, int lessonId)
        {
            throw new System.NotImplementedException();
        }

        public void CheckAccessGroupAndUser(UserIdentityInfo userInfo, int groupId, int userId)
        {
            throw new System.NotImplementedException();
        }

        public void CheckAccessGroupAndTask(UserIdentityInfo userInfo, int groupId, int taskId)
        {
            throw new System.NotImplementedException();
        }
    }
}