using DevEdu.Business.Constants;
using DevEdu.Business.Exceptions;
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

        public void TmpAccess(UserIdentityInfo userInfo, int id2, int id3 = 0)
        {
            throw new AuthorizationException("");
        }
    }
}