using System.Collections.Generic;
using DevEdu.DAL.Enums;
using DevEdu.DAL.Models;

namespace DevEdu.Business.Services
{
    public interface IGroupService
    {
        void AddGroupMaterialReference(int groupId, int materialId, int userId, List<Role> roles);
        void RemoveGroupMaterialReference(int groupId, int materialId, int userId, List<Role> roles);
        int AddGroup(GroupDto groupDto);
        void DeleteGroup(int id);
        GroupDto GetGroup(int id);
        List<GroupDto> GetGroups();
        GroupDto UpdateGroup(int id, GroupDto groupDto);
        GroupDto ChangeGroupStatus(int groupId, int statusId);
        int AddGroupToLesson(int groupId, int lessonId);
        int RemoveGroupFromLesson(int groupId, int lessonId);
        void AddUserToGroup(int groupId, int userId, int roleId);
        void DeleteUserFromGroup(int groupId, int userId);
    }
}