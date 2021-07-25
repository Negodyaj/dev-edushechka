using System.Collections.Generic;
using DevEdu.DAL.Models;

namespace DevEdu.Business.Services
{
    public interface IGroupService
    {
        int AddGroup(GroupDto groupDto);
        void DeleteGroup(int id);
        GroupDto GetGroup(int id);
        List<GroupDto> GetGroups();
        GroupDto UpdateGroup(int id, GroupDto groupDto);
        GroupDto ChangeGroupStatus(int groupId, int statusId);
        void AddGroupLesson(int groupId, int lessonId);
        void RemoveGroupLesson(int groupId, int lessonId);
        int AddGroupMaterialReference(int groupId, int materialId);
        int RemoveGroupMaterialReference(int groupId, int materialId);
        int AddGroupToLesson(int groupId, int materialId);
        int RemoveGroupFromLesson(int groupId, int materialId);
        void AddUserToGroup(int groupId, int userId, int roleId);
        void DeleteUserFromGroup(int groupId, int userId);
        int AddTaskToGroup(int groupId, int taskId, GroupTaskDto dto);
        void DeleteTaskFromGroup(int groupId, int taskId);
        List<GroupTaskDto> GetTasksByGroupId(int groupId);
        GroupTaskDto GetGroupTask(int id, int taskId);
        GroupTaskDto UpdateGroupTask(int groupId, int taskId, GroupTaskDto groupTaskDto);
    }
}