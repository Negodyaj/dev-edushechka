using DevEdu.Business.Constants;
using DevEdu.Business.Exceptions;
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
        private readonly IUserValidationHelper _userValidationHelper;

        public GroupService(IGroupRepository groupRepository)
        {
            _groupRepository = groupRepository;
        }

        public GroupService(IGroupRepository groupRepository, IUserRepository userRepository, IGroupValidationHelper groupValidationHelper,
            IUserValidationHelper userValidationHelper)
        {
            _groupRepository = groupRepository;
            _userRepository = userRepository;
            _groupValidationHelper = groupValidationHelper;
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

        public int AddGroupMaterialReference(int groupId, int materialId) => _groupRepository.AddGroupMaterialReference(groupId, materialId);

        public int RemoveGroupMaterialReference(int groupId, int materialId) => _groupRepository.RemoveGroupMaterialReference(groupId, materialId);
        public int AddGroupToLesson(int groupId, int lessonId) => _groupRepository.AddGroupToLesson(groupId, lessonId);
        public int RemoveGroupFromLesson(int groupId, int lessonId) => _groupRepository.RemoveGroupFromLesson(groupId, lessonId);
        public void AddUserToGroup(int groupId, int userId, int roleId)
        {
            _groupValidationHelper.CheckGroupExistence(groupId);
            var user = _userRepository.SelectUserById(userId);
            if (user ==default)
            {
                throw new EntityNotFoundException(string.Format(ServiceMessages.EntityNotFoundMessage, nameof(user), userId));
            }
            if (!user.Roles.Contains((Role)roleId))
            {
                throw new ValidationException(string.Format(ServiceMessages.UserDoesntHaveRole, userId, (Role)roleId));
            }
            _groupRepository.AddUserToGroup(groupId, userId, roleId);
        }

        public void DeleteUserFromGroup(int groupId, int userId)
        {
            _groupValidationHelper.CheckGroupExistence(groupId);
            var user = _userRepository.SelectUserById(userId);
            if (user == default)
            {
                throw new EntityNotFoundException(string.Format(ServiceMessages.EntityNotFoundMessage, nameof(user), userId));
            }
            _userValidationHelper.CheckUserBelongToGroup(groupId, userId, user.Roles);
            _groupRepository.DeleteUserFromGroup(userId, groupId);
        }

        public int AddTaskToGroup(int groupId, int taskId, GroupTaskDto dto)
        {
            dto.Group = new GroupDto { Id = groupId };
            dto.Task = new TaskDto { Id = taskId };
            return _groupRepository.AddTaskToGroup(dto);
        }

        public void DeleteTaskFromGroup(int groupId, int taskId) => _groupRepository.DeleteTaskFromGroup(groupId, taskId);

        public List<GroupTaskDto> GetTasksByGroupId(int groupId) => _groupRepository.GetTaskGroupByGroupId(groupId);

        public GroupTaskDto GetGroupTask(int groupId, int taskId) => _groupRepository.GetGroupTask(groupId, taskId);

        public GroupTaskDto UpdateGroupTask(int groupId, int taskId, GroupTaskDto dto)
        {
            dto.Group = new GroupDto { Id = groupId };
            dto.Task = new TaskDto { Id = taskId };
            _groupRepository.UpdateGroupTask(dto);
            return _groupRepository.GetGroupTask(groupId, taskId);
        }

    }
}