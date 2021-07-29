using DevEdu.DAL.Enums;
using DevEdu.DAL.Models;
using System.Collections.Generic;

namespace DevEdu.Business.Services
{
    public interface IGroupService
    {
        int AddGroup(GroupDto groupDto);
        void DeleteGroup(int id);
        GroupDto GetGroup(int id, int userId);
        List<GroupDto> GetGroups();
        GroupDto UpdateGroup(int id, GroupDto groupDto, int userId);
        GroupDto ChangeGroupStatus(int groupId, GroupStatus statusId);
        int AddGroupToLesson(int groupId, int lessonId, int userId);
        void RemoveGroupFromLesson(int groupId, int lessonId, int userId);
        int AddGroupMaterialReference(int groupId, int materialId, int userId);
        int RemoveGroupMaterialReference(int groupId, int materialId, int userId);
        void AddUserToGroup(int groupId, int userId, Role roleId, int currentUserId);
        void DeleteUserFromGroup(int userId, int groupId, int currentUserId);
        int AddTaskToGroup(int groupId, int taskId, GroupTaskDto dto, int userId);
        void DeleteTaskFromGroup(int groupId, int taskId, int userId);
        List<GroupTaskDto> GetTasksByGroupId(int groupId, int userId);
        GroupTaskDto GetGroupTask(int id, int taskId, int userId);
        GroupTaskDto UpdateGroupTask(int groupId, int taskId, GroupTaskDto groupTaskDto, int userId);
    }
}