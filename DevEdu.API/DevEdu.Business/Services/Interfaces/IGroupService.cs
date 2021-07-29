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
        GroupDto UpdateGroup(int id, GroupDto groupDto);
        GroupDto ChangeGroupStatus(int groupId, GroupStatus statusId);
        int AddGroupToLesson(int groupId, int lessonId);
        void RemoveGroupFromLesson(int groupId, int lessonId);
        int AddGroupMaterialReference(int groupId, int materialId);
        int RemoveGroupMaterialReference(int groupId, int materialId);
        void AddUserToGroup(int groupId, int userId, Role roleId);
        void DeleteUserFromGroup(int groupId, int userId);
        int AddTaskToGroup(int groupId, int taskId, GroupTaskDto dto);
        void DeleteTaskFromGroup(int groupId, int taskId);
        List<GroupTaskDto> GetTasksByGroupId(int groupId);
        GroupTaskDto GetGroupTask(int id, int taskId);
        GroupTaskDto UpdateGroupTask(int groupId, int taskId, GroupTaskDto groupTaskDto);
    }
}