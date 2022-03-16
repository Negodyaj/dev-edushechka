using DevEdu.Business.IdentityInfo;
using DevEdu.Business.ValidationHelpers;
using DevEdu.DAL.Enums;
using DevEdu.DAL.Models;
using DevEdu.DAL.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DevEdu.Business.Services
{
    public class GroupService : IGroupService
    {
        private readonly IGroupRepository _groupRepository;
        private readonly IUserRepository _userRepository;
        private readonly IGroupValidationHelper _groupHelper;
        private readonly ILessonValidationHelper _lessonHelper;
        private readonly IUserValidationHelper _userHelper;
        private readonly ITaskValidationHelper _taskHelper;

        public GroupService
        (
            IGroupRepository groupRepository,
            IGroupValidationHelper groupHelper,
            IUserRepository userRepository,
            IUserValidationHelper userHelper,
            ILessonValidationHelper lessonHelper,
            ITaskValidationHelper taskHelper
        )
        {
            _groupRepository = groupRepository;
            _userRepository = userRepository;
            _groupHelper = groupHelper;
            _lessonHelper = lessonHelper;
            _userHelper = userHelper;
            _taskHelper = taskHelper;
        }

        public async Task<GroupDto> AddGroup(GroupDto groupDto)
        {
            await _groupHelper.CompareStartEndDateAsync(groupDto.StartDate, groupDto.EndDate);

            groupDto.Id = await _groupRepository.AddGroupAsync(groupDto);
            return groupDto;
        }

        public async Task DeleteGroup(int id)
        {
            await _groupHelper.CheckGroupExistenceAsync(id);

            await _groupRepository.DeleteGroupAsync(id);
        }

        public async Task<GroupDto> GetGroup(int groupId, UserIdentityInfo userInfo)
        {
            await _groupHelper.CheckGroupExistenceAsync(groupId);

            var dto = await _groupRepository.GetGroupAsync(groupId);
            var isAccess =  _groupHelper.CheckAccessGetGroupMembers(groupId, userInfo);
            if (!isAccess || dto == null) return dto;
            dto.Students = await _userRepository.GetUsersByGroupIdAndRoleAsync(groupId, (int)Role.Student);
            dto.Tutors = await _userRepository.GetUsersByGroupIdAndRoleAsync(groupId, (int)Role.Tutor);
            dto.Teachers = await _userRepository.GetUsersByGroupIdAndRoleAsync(groupId, (int)Role.Teacher);
            return dto;
        }

        public async Task<List<GroupDto>> GetGroups() => await _groupRepository.GetGroupsAsync();

        public async Task<int> AddGroupLesson(int groupId, int lessonId)
        {
            await _groupHelper.CheckGroupExistenceAsync(groupId);
            await _lessonHelper.GetLessonByIdAndThrowIfNotFoundAsync(lessonId);

            return await _groupRepository.AddGroupToLessonAsync(groupId, lessonId);
        }

        public async Task RemoveGroupLesson(int groupId, int lessonId)
        {
            await _groupHelper.CheckGroupExistenceAsync(groupId);
            await _lessonHelper.GetLessonByIdAndThrowIfNotFoundAsync(lessonId);

            await _groupRepository.RemoveGroupFromLessonAsync(groupId, lessonId);
        }

        public async Task<GroupDto> UpdateGroup(int id, GroupDto groupDto, UserIdentityInfo userInfo)
        {
            await _groupHelper.CheckGroupExistenceAsync(id);
            await _groupHelper.CompareStartEndDateAsync(groupDto.StartDate, groupDto.EndDate);
            groupDto.Id = id;
            return await _groupRepository.UpdateGroupAsync(groupDto);
        }

        public async Task<GroupDto> ChangeGroupStatus(int groupId, GroupStatus statusId)
        {
            await _groupHelper.CheckGroupExistenceAsync(groupId);

            return await _groupRepository.ChangeGroupStatusAsync(groupId, (int)statusId);
        }

        public async Task<int> AddGroupToLesson(int groupId, int lessonId, UserIdentityInfo userInfo)
        {
            await _groupHelper.CheckGroupExistenceAsync(groupId);
            await _lessonHelper.GetLessonByIdAndThrowIfNotFoundAsync(lessonId);

            return await _groupRepository.AddGroupToLessonAsync(groupId, lessonId);
        }

        public async Task RemoveGroupFromLesson(int groupId, int lessonId, UserIdentityInfo userInfo)
        {
            await _groupHelper.CheckGroupExistenceAsync(groupId);
            await _lessonHelper.GetLessonByIdAndThrowIfNotFoundAsync(lessonId);

            await _groupRepository.RemoveGroupFromLessonAsync(groupId, lessonId);
        }

        public async Task AddUserToGroup(int groupId, int userId, Role roleId, UserIdentityInfo userInfo)
        {
            await _groupHelper.CheckGroupExistenceAsync(groupId);
            await _userHelper.GetUserByIdAndThrowIfNotFoundAsync(userId);

            await _groupRepository.AddUserToGroupAsync(groupId, userId, (int)roleId);
        }

        public async Task DeleteUserFromGroup(int userId, int groupId, UserIdentityInfo userInfo)
        {
            await _groupHelper.CheckGroupExistenceAsync(groupId);
            await _userHelper.GetUserByIdAndThrowIfNotFoundAsync(userId);

            await _groupRepository.DeleteUserFromGroupAsync(userId, groupId);
        }

        public async Task DeleteTaskFromGroup(int groupId, int taskId, UserIdentityInfo userInfo)
        {
            await _groupHelper.CheckGroupExistenceAsync(groupId);
            await _taskHelper.GetTaskByIdAndThrowIfNotFoundAsync(taskId);

            await _groupRepository.DeleteTaskFromGroupAsync(groupId, taskId);
        }
    }
}