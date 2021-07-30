using System.Collections.Generic;
using DevEdu.DAL.Models;

namespace DevEdu.DAL.Repositories
{
    public interface IGroupRepository
    {
        int AddGroup(GroupDto groupDto);
        void DeleteGroup(int id);
        GroupDto GetGroup(int id);
        List<GroupDto> GetGroups();
        GroupDto UpdateGroup(int id, GroupDto groupDto);
        int AddUserToGroup(int groupId, int userId, int roleId);
        int DeleteUserFromGroup(int userId, int groupId);
        int AddGroupToLesson(int groupId, int lessonId);
        int RemoveGroupFromLesson(int groupId, int lessonId);
        GroupDto ChangeGroupStatus(int groupId, int statusId);
        int AddGroupMaterialReference(int groupId, int materialId);
        int RemoveGroupMaterialReference(int groupId, int materialId);
        int AddTaskToGroup(GroupTaskDto groupTaskDto);
        void DeleteTaskFromGroup(int groupId, int taskId);
        List<GroupTaskDto> GetTaskGroupByGroupId(int groupId);
        GroupTaskDto GetGroupTask(int groupId, int taskId);
        void UpdateGroupTask(GroupTaskDto groupTaskDto);
        public List<GroupDto> GetGroupsByMaterialId(int id);
        public List<GroupDto> GetGroupsByTaskId(int taskId);
        public List<GroupDto> GetGroupsByUserId(int userId);
    }
}