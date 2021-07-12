using DevEdu.DAL.Models;
using System.Collections.Generic;

namespace DevEdu.Business.Services
{
    public interface IGroupService
    {
        GroupDto AddGroup(GroupDto groupDto);
        void DeleteGroup(int id);
        GroupDto GetGroup(int id);
        List<GroupDto> GetGroups();
        GroupDto UpdateGroup(int id, GroupDto groupDto);
        int AddGroupMaterialReference(int groupId, int materialId);
        int RemoveGroupMaterialReference(int groupId, int materialId);
        void AddUserToGroup(int groupId, int userId, int roleId);
        void DeleteUserFromGroup(int groupId, int userId);
    }
}