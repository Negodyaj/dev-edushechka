using DevEdu.Business.Constants;
using DevEdu.Business.Exceptions;
using DevEdu.Business.IdentityInfo;
using DevEdu.DAL.Models;
using DevEdu.DAL.Repositories;
using System.Linq;
using System.Threading.Tasks;
using DevEdu.DAL.Enums;

namespace DevEdu.Business.ValidationHelpers
{
    public class GroupValidationHelper : IGroupValidationHelper
    {
        private readonly IGroupRepository _groupRepository;

        public GroupValidationHelper(IGroupRepository groupRepository)
        {
            _groupRepository = groupRepository;
        }

        public async Task<GroupDto> CheckGroupExistenceAsync(int groupId)
        {
            var group = await _groupRepository.GetGroup(groupId);
            if (group == default)
                throw new EntityNotFoundException(string.Format(ServiceMessages.EntityNotFoundMessage, nameof(group), groupId));
            return group;
        }

        public void CheckUserInGroupExistence(int groupId, int userId)
        {
            var groupsByUser = _groupRepository.GetGroupsByUserId(userId);
            //var group = Task.Run(async () => await _groupRepository.GetGroup(groupId)).Result;
            var result = groupsByUser.FirstOrDefault(gu => gu.Id == groupId);
            if (result == default)
                throw new AuthorizationException(string.Format(ServiceMessages.UserInGroupNotFoundMessage, userId, groupId));
        }

        public bool CheckAccessGetGroupMembers(int groupId, UserIdentityInfo userInfo)
        {
            bool isAccess = false;
            foreach (var role in userInfo.Roles)
            {
                if (role is Role.Manager or Role.Admin)
                {
                    isAccess = true;
                }
            }
            return isAccess;
        }

        public void CheckAccessGroup(UserIdentityInfo userInfo, int groupId)
        {
            throw new System.NotImplementedException();
        }

        public void CheckAccessGroupAndLesson(UserIdentityInfo userInfo, int groupId, int lessonId)
        {
            throw new System.NotImplementedException();
        }

        public void CheckAccessGroupAndMaterial(UserIdentityInfo userInfo, int groupId, int materialId)
        {
            throw new System.NotImplementedException();
        }

        public void CheckAccessGroupAndTask(UserIdentityInfo userInfo, int groupId, int taskId)
        {
            throw new System.NotImplementedException();
        }

        public void CheckAccessGroupAndUser(UserIdentityInfo userInfo, int groupId, int userId)
        {
            throw new System.NotImplementedException();
        }
    }
}