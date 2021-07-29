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
        Task<GroupDto> GetGroup(int id, int userId);
        Task<List<GroupDto>> GetGroups();
        Task<GroupDto> UpdateGroup(int id, GroupDto groupDto, int userId);
        Task<GroupDto> ChangeGroupStatus(int groupId, GroupStatus statusId);
        Task<int> AddGroupToLesson(int groupId, int lessonId, int userId);
        Task RemoveGroupFromLesson(int groupId, int lessonId, int userId);
        Task<int> AddGroupMaterialReference(int groupId, int materialId, int userId);
        Task<int> RemoveGroupMaterialReference(int groupId, int materialId, int userId);
        Task AddUserToGroup(int groupId, int userId, Role roleId, int currentUserId);
        Task DeleteUserFromGroup(int userId, int groupId, int currentUserId);
        Task<int> AddTaskToGroup(int groupId, int taskId, GroupTaskDto dto, int userId);
        Task DeleteTaskFromGroup(int groupId, int taskId, int userId);
        Task<List<GroupTaskDto>> GetTasksByGroupId(int groupId, int userId);
        Task<GroupTaskDto> GetGroupTask(int id, int taskId, int userId);
        Task<GroupTaskDto> UpdateGroupTask(int groupId, int taskId, GroupTaskDto groupTaskDto, int userId);
    }
}