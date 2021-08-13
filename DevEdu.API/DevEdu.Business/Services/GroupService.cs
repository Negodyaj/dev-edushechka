using DevEdu.Business.Constants;
using DevEdu.Business.Exceptions;
using DevEdu.Business.IdentityInfo;
using DevEdu.Business.ValidationHelpers;
using DevEdu.DAL.Enums;
using DevEdu.DAL.Models;
using DevEdu.DAL.Repositories;
using System.Collections.Generic;

namespace DevEdu.Business.Services
{
    public class GroupService : IGroupService
    {
        private readonly IGroupRepository _groupRepository;
        private readonly IUserRepository _userRepository;
        private readonly IGroupValidationHelper _groupValidationHelper;
        private readonly IMaterialValidationHelper _materialValidationHelper;
        private readonly IUserValidationHelper _userValidationHelper;

        public GroupService
        (
            IGroupRepository groupRepository,
            IUserRepository userRepository,
            IGroupValidationHelper groupValidationHelper,
            IMaterialValidationHelper materialValidationHelper,
            IUserValidationHelper userValidationHelper
        )
        {
            _groupRepository = groupRepository;
            _userRepository = userRepository;
            _groupValidationHelper = groupValidationHelper;
            _materialValidationHelper = materialValidationHelper;
            _userValidationHelper = userValidationHelper;
        }

        public int AddGroup(GroupDto groupDto) => _groupRepository.AddGroup(groupDto);

        public void DeleteGroup(int id) => _groupRepository.DeleteGroup(id);

        public GroupDto GetGroup(int id)
        {
            var dto = _groupRepository.GetGroup(id);
            dto.Students = _userRepository.GetUsersByGroupIdAndRole(id, (int)Role.Student);
            dto.Tutors = _userRepository.GetUsersByGroupIdAndRole(id, (int)Role.Tutor);
            dto.Teachers = _userRepository.GetUsersByGroupIdAndRole(id, (int)Role.Teacher);
            return dto;
        }

        public List<GroupDto> GetGroups() => _groupRepository.GetGroups();

        public int AddGroupLesson(int groupId, int lessonId) => _groupRepository.AddGroupToLesson(groupId, lessonId);

        public int RemoveGroupLesson(int groupId, int lessonId) => _groupRepository.RemoveGroupFromLesson(groupId, lessonId);

        public GroupDto UpdateGroup(int id, GroupDto groupDto) => _groupRepository.UpdateGroup(id, groupDto);
        public GroupDto ChangeGroupStatus(int groupId, int statusId) => _groupRepository.ChangeGroupStatus(groupId, statusId);

        public void AddGroupMaterialReference(int groupId, int materialId, UserIdentityInfo userInfo)
        {
            CheckAccessAndExistenceAndThrowIfNotFound(groupId, materialId, userInfo);
            _groupRepository.AddGroupMaterialReference(groupId, materialId);
        }

        public void RemoveGroupMaterialReference(int groupId, int materialId, UserIdentityInfo userInfo)
        {
            CheckAccessAndExistenceAndThrowIfNotFound(groupId, materialId, userInfo);
            _groupRepository.RemoveGroupMaterialReference(groupId, materialId);
        }

        public int AddGroupToLesson(int groupId, int lessonId) => _groupRepository.AddGroupToLesson(groupId, lessonId);
        public int RemoveGroupFromLesson(int groupId, int lessonId) => _groupRepository.RemoveGroupFromLesson(groupId, lessonId);
        public void AddUserToGroup(int groupId, int userId, int roleId)
        {
            _groupValidationHelper.CheckGroupExistence(groupId);
            var user = _userRepository.GetUserById(userId);
            if (user == default)
            {
                throw new EntityNotFoundException(string.Format(ServiceMessages.EntityNotFoundMessage, nameof(user), userId));
            }
            _groupRepository.AddUserToGroup(groupId, userId, roleId);
        }

        public void DeleteUserFromGroup(int groupId, int userId)
        {
            _groupValidationHelper.CheckGroupExistence(groupId);
            var user = _userRepository.GetUserById(userId);
            if (user == default)
            {
                throw new EntityNotFoundException(string.Format(ServiceMessages.EntityNotFoundMessage, nameof(user), userId));
            }
            _userValidationHelper.CheckUserBelongToGroup(groupId, userId, user.Roles);
            _groupRepository.DeleteUserFromGroup(userId, groupId);
        }

        private void CheckAccessAndExistenceAndThrowIfNotFound(int groupId, int materialId, UserIdentityInfo userInfo)
        {
            var userId = userInfo.UserId;
            _groupValidationHelper.CheckGroupExistence(groupId);
            _materialValidationHelper.GetMaterialByIdAndThrowIfNotFound(materialId);
            if (!userInfo.IsAdmin())
                _groupValidationHelper.CheckUserInGroupExistence(groupId, userId);
        }
    }
}