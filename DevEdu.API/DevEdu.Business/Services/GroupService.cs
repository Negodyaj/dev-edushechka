using DevEdu.DAL.Enums;
using DevEdu.DAL.Models;
using DevEdu.DAL.Repositories;
using System.Collections.Generic;
using DevEdu.Business.ValidationHelpers;

namespace DevEdu.Business.Services
{
    public class GroupService : IGroupService
    {
        private readonly IGroupRepository _groupRepository;
        private readonly IUserRepository _userRepository;
        private readonly IGroupValidationHelper _groupValidationHelper;
        private readonly IMaterialValidationHelper _materialValidationHelper;

        public GroupService
        (
            IGroupRepository groupRepository,
            IUserRepository userRepository,
            IGroupValidationHelper groupValidationHelper,
            IMaterialValidationHelper materialValidationHelper
        )
        {
            _groupRepository = groupRepository;
            _userRepository = userRepository;
            _groupValidationHelper = groupValidationHelper;
            _materialValidationHelper = materialValidationHelper;
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

        public void AddGroupMaterialReference(int groupId, int materialId, UserIdentityInfo userIdentityInfo)
        {
            var userId = userIdentityInfo.UserId;
            var roles = userIdentityInfo.Roles;
            CheckAccessAndExistence(groupId, materialId, userId, roles);

            _groupRepository.AddGroupMaterialReference(groupId, materialId);
        }

        public void RemoveGroupMaterialReference(int groupId, int materialId, UserIdentityInfo userIdentityInfo)
        {
            var userId = userIdentityInfo.UserId;
            var roles = userIdentityInfo.Roles;
            CheckAccessAndExistence(groupId, materialId, userId, roles);

            _groupRepository.RemoveGroupMaterialReference(groupId, materialId);
        }

        public int AddGroupToLesson(int groupId, int lessonId) => _groupRepository.AddGroupToLesson(groupId, lessonId);
        public int RemoveGroupFromLesson(int groupId, int lessonId) => _groupRepository.RemoveGroupFromLesson(groupId, lessonId);  
        public void AddUserToGroup(int groupId, int userId, int roleId) => _groupRepository.AddUserToGroup(groupId, userId, roleId);
        public void DeleteUserFromGroup(int groupId, int userId) => _groupRepository.DeleteUserFromGroup(userId, groupId);

        private void CheckAccessAndExistence(int groupId, int materialId, int userId, List<Role> roles)
        {
            if (!CheckerRole.IsAdmin(roles))
                _groupValidationHelper.CheckUserInGroupExistence(groupId, userId);

            _groupValidationHelper.CheckGroupExistence(groupId);
            _materialValidationHelper.CheckMaterialExistence(materialId);
        }
    }
}