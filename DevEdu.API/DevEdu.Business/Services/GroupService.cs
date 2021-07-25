using DevEdu.DAL.Models;
using DevEdu.DAL.Repositories;
using System.Collections.Generic;

namespace DevEdu.Business.Services
{
    public class GroupService : IGroupService
    {
        private readonly IGroupRepository _groupRepository;
        public GroupService(IGroupRepository groupRepository)
        {
            _groupRepository = groupRepository;
        }

        public int AddGroupMaterialReference(int groupId, int materialId) => _groupRepository.AddGroupMaterialReference(groupId, materialId);

        public int RemoveGroupMaterialReference(int groupId, int materialId) => _groupRepository.RemoveGroupMaterialReference(groupId, materialId);
        public int AddGroupToLesson(int groupId, int lessonId) => _groupRepository.AddGroupToLesson(groupId, lessonId);
        public int RemoveGroupFromLesson(int groupId, int lessonId) => _groupRepository.RemoveGroupFromLesson(groupId, lessonId);  
        public void AddUserToGroup(int groupId, int userId, int roleId) => _groupRepository.AddUserToGroup(groupId, userId, roleId);

        public void DeleteUserFromGroup(int groupId, int userId) => _groupRepository.DeleteUserFromGroup(userId, groupId);

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