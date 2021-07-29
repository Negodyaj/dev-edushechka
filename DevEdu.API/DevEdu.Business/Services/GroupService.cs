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
        private readonly IGroupValidationHelper _groupHelper;
        private readonly ILessonValidationHelper _lessonHelper;
        private readonly IMaterialValidationHelper _materialHelper;
        private readonly IUserValidationHelper _userHelper;
        private readonly ITaskValidationHelper _taskHelper;

        public GroupService
        (
            IGroupRepository groupRepository,
            IGroupValidationHelper groupHelper,
            IUserRepository userRepository = default,
            ILessonValidationHelper lessonHelper = default,
            IMaterialValidationHelper materialHelper = default,
            IUserValidationHelper userHelper = default,
            ITaskValidationHelper taskHelper = default
        )
        {
            _groupRepository = groupRepository;
            _userRepository = userRepository;
            _groupHelper = groupHelper;
            _lessonHelper = lessonHelper;
            _materialHelper = materialHelper;
            _userHelper = userHelper;
            _taskHelper = taskHelper;
        }

        public int AddGroup(GroupDto groupDto) => _groupRepository.AddGroup(groupDto);

        public void DeleteGroup(int id)
        {
            _groupHelper.CheckGroupExistence(id);

            _groupRepository.DeleteGroup(id);
        }

        public GroupDto GetGroup(int groupId, int userId)
        {
            _groupHelper.CheckGroupExistence(groupId);
            _groupHelper.CheckAccessGetGroupMembers(groupId, userId);

            var dto = _groupRepository.GetGroup(groupId);
            if (dto != null)
            {
                dto.Students = _userRepository.GetUsersByGroupIdAndRole(groupId, (int)Role.Student);
                dto.Tutors = _userRepository.GetUsersByGroupIdAndRole(groupId, (int)Role.Tutor);
                dto.Teachers = _userRepository.GetUsersByGroupIdAndRole(groupId, (int)Role.Teacher);
            }
            return dto;
        }

        public List<GroupDto> GetGroups() => _groupRepository.GetGroups();

        public int AddGroupLesson(int groupId, int lessonId)
        {
            _groupHelper.CheckGroupExistence(groupId);
            _lessonHelper.CheckLessonExistence(lessonId);

            return _groupRepository.AddGroupToLesson(groupId, lessonId);
        }

        public void RemoveGroupLesson(int groupId, int lessonId)
        {
            _groupHelper.CheckGroupExistence(groupId);
            _lessonHelper.CheckLessonExistence(lessonId);

            _groupRepository.RemoveGroupFromLesson(groupId, lessonId);
        }

        public GroupDto UpdateGroup(int id, GroupDto groupDto, int userId)
        {
            _groupHelper.CheckGroupExistence(id);
            _groupHelper.TmpAccess(id, userId);

            groupDto.Id = id;
            return _groupRepository.UpdateGroup(groupDto);
        }

        public GroupDto ChangeGroupStatus(int groupId, GroupStatus statusId)
        {
            _groupHelper.CheckGroupExistence(groupId);

            return _groupRepository.ChangeGroupStatus(groupId, (int)statusId);
        }

        public int AddGroupMaterialReference(int groupId, int materialId, int userId)
        {
            _groupHelper.CheckGroupExistence(groupId);
            _materialHelper.CheckMaterialExistence(materialId);
            _groupHelper.TmpAccess(groupId, materialId, userId);

            return _groupRepository.AddGroupMaterialReference(groupId, materialId);
        }

        public int RemoveGroupMaterialReference(int groupId, int materialId, int userId)
        {
            _groupHelper.CheckGroupExistence(groupId);
            _materialHelper.CheckMaterialExistence(materialId);
            _groupHelper.TmpAccess(groupId, materialId, userId);

            return _groupRepository.RemoveGroupMaterialReference(groupId, materialId);
        }

        public int AddGroupToLesson(int groupId, int lessonId, int userId)
        {
            _groupHelper.CheckGroupExistence(groupId);
            _lessonHelper.CheckLessonExistence(lessonId);
            _groupHelper.TmpAccess(groupId, lessonId, userId);

            return _groupRepository.AddGroupToLesson(groupId, lessonId);
        }

        public void RemoveGroupFromLesson(int groupId, int lessonId, int userId)
        {
            _groupHelper.CheckGroupExistence(groupId);
            _lessonHelper.CheckLessonExistence(lessonId);
            _groupHelper.TmpAccess(groupId, lessonId, userId);

            _groupRepository.RemoveGroupFromLesson(groupId, lessonId);
        }

        public void AddUserToGroup(int groupId, int userId, Role roleId, int currentUserId)
        {
            _groupHelper.CheckGroupExistence(groupId);
            _userHelper.CheckUserExistence(userId);
            _groupHelper.TmpAccess(groupId, userId, currentUserId);


            _groupRepository.AddUserToGroup(groupId, userId, (int)roleId);
        }

        public void DeleteUserFromGroup(int userId, int groupId, int currentUserId)
        {
            _groupHelper.CheckGroupExistence(groupId);
            _userHelper.CheckUserExistence(userId);
            _groupHelper.TmpAccess(userId, groupId, currentUserId);

            _groupRepository.DeleteUserFromGroup(userId, groupId);
        }


        public int AddTaskToGroup(int groupId, int taskId, GroupTaskDto dto, int userId)
        {
            _groupHelper.CheckGroupExistence(groupId);
            _groupHelper.TmpAccess(groupId, taskId, userId);
            _taskHelper.CheckTaskExistence(taskId);

            dto.Group = new GroupDto { Id = groupId };
            dto.Task = new TaskDto { Id = taskId };
            return _groupRepository.AddTaskToGroup(dto);
        }

        public void DeleteTaskFromGroup(int groupId, int taskId, int userId)
        {
            _groupHelper.CheckGroupExistence(groupId);
            _groupHelper.TmpAccess(groupId, taskId, userId);
            _taskHelper.CheckTaskExistence(taskId);

            _groupRepository.DeleteTaskFromGroup(groupId, taskId);
        }

        public List<GroupTaskDto> GetTasksByGroupId(int groupId, int userId)
        {
            _groupHelper.CheckGroupExistence(groupId);
            _groupHelper.TmpAccess(groupId, userId);

            return _groupRepository.GetTaskGroupByGroupId(groupId);
        }

        public GroupTaskDto GetGroupTask(int groupId, int taskId, int userId)
        {
            _groupHelper.CheckGroupExistence(groupId);
            _groupHelper.TmpAccess(groupId, taskId, userId);
            _taskHelper.CheckTaskExistence(taskId);

            return _groupRepository.GetGroupTask(groupId, taskId);
        }

        public GroupTaskDto UpdateGroupTask(int groupId, int taskId, GroupTaskDto dto, int userId)
        {
            _groupHelper.CheckGroupExistence(groupId);
            _groupHelper.TmpAccess(groupId, taskId, userId);
            _taskHelper.CheckTaskExistence(taskId);

            dto.Group = new GroupDto { Id = groupId };
            dto.Task = new TaskDto { Id = taskId };
            _groupRepository.UpdateGroupTask(dto);
            return _groupRepository.GetGroupTask(groupId, taskId);
        }
    }
}