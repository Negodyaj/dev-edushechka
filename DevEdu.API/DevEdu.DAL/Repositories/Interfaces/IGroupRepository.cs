using DevEdu.DAL.Models;
using System.Collections.Generic;

namespace DevEdu.DAL.Repositories
{
    public interface IGroupRepository
    {
        GroupDto AddGroup(GroupDto groupDto);
        void DeleteGroup(int id);
        GroupDto GetGroup(int id);
        List<GroupDto> GetGroups();
        GroupDto UpdateGroup(int id, GroupDto groupDto);
        int AddUserToGroup(int groupId, int userId, int roleId);
        int DeleteUserFromGroup(int userId, int groupId);
        void AddGroupLesson(int groupId, int lessonId);
        void RemoveGroupLesson(int groupId, int lessonId);
        int AddGroupMaterialReference(int groupId, int materialId);
        int RemoveGroupMaterialReference(int groupId, int materialId);
    }
}