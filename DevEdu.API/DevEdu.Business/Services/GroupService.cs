using DevEdu.Business.ValidationHelpers;
using DevEdu.DAL.Enums;
using DevEdu.DAL.Models;
using DevEdu.DAL.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;
using static Dapper.SqlMapper;

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

        public async Task<int> AddGroup(GroupDto groupDto) => await _groupRepository.AddGroup(groupDto);

        public async Task DeleteGroup(int id)
        {
            _groupHelper.CheckGroupExistence(id);

            await _groupRepository.DeleteGroup(id);
        }

        public async Task<GroupDto> GetGroup(int groupId, int userId)
        {
            _groupHelper.CheckGroupExistence(groupId);
            _groupHelper.CheckAccessGetGroupMembers(groupId, userId);

            var dto = await _groupRepository.GetGroup(groupId);
            if (dto != null)
            {
                var tasks = new List<Task>
                {
                    Task.Run(() => { dto.Students =
                        _userRepository.GetUsersByGroupIdAndRole(groupId, (int)Role.Student);}),
                    Task.Run(() => { dto.Tutors =
                        _userRepository.GetUsersByGroupIdAndRole(groupId, (int)Role.Tutor);}),
                    Task.Run(() => { dto.Teachers =
                        _userRepository.GetUsersByGroupIdAndRole(groupId, (int)Role.Teacher);})
                };
                await Task.WhenAll(tasks);
            }
            return dto;
        }

        public async Task<List<GroupDto>> GetGroups() => await _groupRepository.GetGroups();

        public async Task<int> AddGroupLesson(int groupId, int lessonId)
        {
            _groupHelper.CheckGroupExistence(groupId);
            _lessonHelper.CheckLessonExistence(lessonId);

            return await _groupRepository.AddGroupToLesson(groupId, lessonId);
        }

        public async Task RemoveGroupLesson(int groupId, int lessonId)
        {
            _groupHelper.CheckGroupExistence(groupId);
            _lessonHelper.CheckLessonExistence(lessonId);

            await _groupRepository.RemoveGroupFromLesson(groupId, lessonId);
        }

        public async Task<GroupDto> UpdateGroup(int id, GroupDto groupDto, int userId)
        {
            _groupHelper.CheckGroupExistence(id);
            _groupHelper.TmpAccess(id, userId);

            groupDto.Id = id;
            return await _groupRepository.UpdateGroup(groupDto);
        }

        public async Task<GroupDto> ChangeGroupStatus(int groupId, GroupStatus statusId)
        {
            _groupHelper.CheckGroupExistence(groupId);

            return await _groupRepository.ChangeGroupStatus(groupId, (int)statusId);
        }

        public async Task<int> AddGroupMaterialReference(int groupId, int materialId, int userId)
        {
            _groupHelper.CheckGroupExistence(groupId);
            _materialHelper.CheckMaterialExistence(materialId);
            _groupHelper.TmpAccess(groupId, materialId, userId);

            return await _groupRepository.AddGroupMaterialReference(groupId, materialId);
        }

        public async Task<int> RemoveGroupMaterialReference(int groupId, int materialId, int userId)
        {
            _groupHelper.CheckGroupExistence(groupId);
            _materialHelper.CheckMaterialExistence(materialId);
            _groupHelper.TmpAccess(groupId, materialId, userId);

            return await _groupRepository.RemoveGroupMaterialReference(groupId, materialId);
        }

        public async Task<int> AddGroupToLesson(int groupId, int lessonId, int userId)
        {
            _groupHelper.CheckGroupExistence(groupId);
            _lessonHelper.CheckLessonExistence(lessonId);
            _groupHelper.TmpAccess(groupId, lessonId, userId);

            return await _groupRepository.AddGroupToLesson(groupId, lessonId);
        }

        public async Task RemoveGroupFromLesson(int groupId, int lessonId, int userId)
        {
            _groupHelper.CheckGroupExistence(groupId);
            _lessonHelper.CheckLessonExistence(lessonId);
            _groupHelper.TmpAccess(groupId, lessonId, userId);

            await _groupRepository.RemoveGroupFromLesson(groupId, lessonId);
        }

        public async Task AddUserToGroup(int groupId, int userId, Role roleId, int currentUserId)
        {
            _groupHelper.CheckGroupExistence(groupId);
            _userHelper.CheckUserExistence(userId);
            _groupHelper.TmpAccess(groupId, userId, currentUserId);


            await _groupRepository.AddUserToGroup(groupId, userId, (int)roleId);
        }

        public async Task DeleteUserFromGroup(int userId, int groupId, int currentUserId)
        {
            _groupHelper.CheckGroupExistence(groupId);
            _userHelper.CheckUserExistence(userId);
            _groupHelper.TmpAccess(userId, groupId, currentUserId);

            await _groupRepository.DeleteUserFromGroup(userId, groupId);
        }


        public async Task<int> AddTaskToGroup(int groupId, int taskId, GroupTaskDto dto, int userId)
        {
            _groupHelper.CheckGroupExistence(groupId);
            _groupHelper.TmpAccess(groupId, taskId, userId);
            _taskHelper.CheckTaskExistence(taskId);

            dto.Group = new GroupDto { Id = groupId };
            dto.Task = new TaskDto { Id = taskId };
            return await _groupRepository.AddTaskToGroup(dto);
        }

        public async Task DeleteTaskFromGroup(int groupId, int taskId, int userId)
        {
            _groupHelper.CheckGroupExistence(groupId);
            _groupHelper.TmpAccess(groupId, taskId, userId);
            _taskHelper.CheckTaskExistence(taskId);

            await _groupRepository.DeleteTaskFromGroup(groupId, taskId);
        }

        public async Task<List<GroupTaskDto>> GetTasksByGroupId(int groupId, int userId)
        {
            _groupHelper.CheckGroupExistence(groupId);
            _groupHelper.TmpAccess(groupId, userId);

            return await _groupRepository.GetTaskGroupByGroupId(groupId);
        }

        public async Task<GroupTaskDto> GetGroupTask(int groupId, int taskId, int userId)
        {
            _groupHelper.CheckGroupExistence(groupId);
            _groupHelper.TmpAccess(groupId, taskId, userId);
            _taskHelper.CheckTaskExistence(taskId);

            return await _groupRepository.GetGroupTask(groupId, taskId);
        }

        public async Task<GroupTaskDto> UpdateGroupTask(int groupId, int taskId, GroupTaskDto dto, int userId)
        {
            _groupHelper.CheckGroupExistence(groupId);
            _groupHelper.TmpAccess(groupId, taskId, userId);
            _taskHelper.CheckTaskExistence(taskId);

            dto.Group = new GroupDto { Id = groupId };
            dto.Task = new TaskDto { Id = taskId };
            await _groupRepository.UpdateGroupTask(dto);
            return await _groupRepository.GetGroupTask(groupId, taskId);
        }
    }
}