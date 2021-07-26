using System.Collections.Generic;
using DevEdu.DAL.Enums;
using DevEdu.DAL.Models;

namespace DevEdu.DAL.Repositories
{
    public interface IGroupRepository
    {
        int AddUserToGroup(int groupId, int userId, int roleId);
        int DeleteUserFromGroup(int userId, int groupId);
        int AddGroupToLesson(int groupId, int lessonId);
        int RemoveGroupFromLesson(int groupId, int lessonId);
        int AddGroupMaterialReference(int groupId, int materialId);
        int RemoveGroupMaterialReference(int groupId, int materialId);
        int AddTaskToGroup(GroupTaskDto groupTaskDto);
        void DeleteTaskFromGroup(int groupId, int taskId);
        List<GroupTaskDto> GetTaskGroupByGroupId(int groupId);
        GroupTaskDto GetGroupTask(int groupId, int taskId);
        void UpdateGroupTask(GroupTaskDto groupTaskDto);
        public List<GroupDto> GetGroupsByMaterialId(int id);
        public int GetUser_GroupByUserIdAndUserRoleAndGroupId(int userId, Role roleId, int groupId);
    }
}