using System.Collections.Generic;
using DevEdu.DAL.Models;

namespace DevEdu.DAL.Repositories
{
    public interface IGroupRepository
    {
        int AddUserToGroup(int groupId, int userId, int roleId);
        int DeleteUserFromGroup(int userId, int groupId);
        void AddGroupLesson(int groupId, int lessonId);
        void RemoveGroupLesson(int groupId, int lessonId);
        int AddGroupMaterialReference(int groupId, int materialId);
        int RemoveGroupMaterialReference(int groupId, int materialId);
        int AddTaskToGroup(GroupTaskDto groupTaskDto);
        void DeleteTaskFromGroup(int groupId, int taskId);
        List<GroupTaskDto> GetTaskGroupByGroupId(int groupId);
        GroupTaskDto GetGroupTask(int groupId, int taskId);
        GroupTaskDto UpdateGroupTask(GroupTaskDto groupTaskDto);
    }
}