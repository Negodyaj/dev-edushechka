using DevEdu.Business.IdentityInfo;
using DevEdu.DAL.Enums;
using DevEdu.DAL.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DevEdu.Business.Services
{
    public interface IGroupService
    {
        Task<int> AddGroup(GroupDto groupDto);
        Task DeleteGroup(int id);
        Task<GroupDto> GetGroup(int id, UserIdentityInfo userInfo);
        Task<List<GroupDto>> GetGroups();
        Task<GroupDto> UpdateGroup(int id, GroupDto groupDto, UserIdentityInfo userInfo);
        Task<GroupDto> ChangeGroupStatus(int groupId, GroupStatus statusId);
        Task<int> AddGroupToLesson(int groupId, int lessonId, UserIdentityInfo userInfo);
        Task RemoveGroupFromLesson(int groupId, int lessonId, UserIdentityInfo userInfo);
        Task<int> AddGroupMaterialReference(int groupId, int materialId, UserIdentityInfo userInfo);
        Task<int> RemoveGroupMaterialReference(int groupId, int materialId, UserIdentityInfo userInfo);
        Task AddUserToGroup(int groupId, int userId, Role roleId, UserIdentityInfo userInfo);
        Task DeleteUserFromGroup(int userId, int groupId, UserIdentityInfo userInfo);
        Task DeleteTaskFromGroup(int groupId, int taskId, UserIdentityInfo userInfo);
    }
}