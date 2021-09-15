﻿using DevEdu.Business.Constants;
using DevEdu.Business.Exceptions;
using DevEdu.Business.IdentityInfo;
using DevEdu.DAL.Enums;
using DevEdu.DAL.Models;
using DevEdu.DAL.Repositories;
using System.Linq;
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

        public async Task<GroupDto> CheckGroupExistenceAsync(int groupId)
        {
            var group = await _groupRepository.GetGroup(groupId);
            if (group == default)
                throw new EntityNotFoundException(string.Format(ServiceMessages.EntityWithIdNotFoundMessage, nameof(group), groupId));
            return group;
        }

        public void CheckUserInGroupExistence(int groupId, int userId)
        {
            var groupsByUser = _groupRepository.GetGroupsByUserId(userId);
            var result = groupsByUser.FirstOrDefault(gu => gu.Id == groupId);
            if (result == default)
                throw new AuthorizationException(string.Format(ServiceMessages.UserInGroupNotFoundMessage, userId, groupId));
        }

        public bool CheckAccessGetGroupMembers(int groupId, UserIdentityInfo userInfo)
        {
            var isAccess = false;
            foreach (var role in userInfo.Roles.Where(role => role is Role.Manager or Role.Admin))
            {
                isAccess = true;
            }
            return isAccess;
        }
    }
}