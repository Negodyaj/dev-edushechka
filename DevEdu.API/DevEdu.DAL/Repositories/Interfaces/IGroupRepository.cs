using System.Collections.Generic;
using DevEdu.DAL.Enums;
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
        void AddGroupMaterialReference(int groupId, int materialId);
        void RemoveGroupMaterialReference(int groupId, int materialId);
        public List<GroupDto> GetGroupsByMaterialId(int id);
        int GetPresentGroupForStudentByUserId(int userId);
        List<GroupDto> GetGroupsByTaskId(int taskId);
        List<GroupDto> GetGroupsByUserId(int userId);
        List<GroupDto> GetGroupsByLessonId(int lessonId);
    }
}