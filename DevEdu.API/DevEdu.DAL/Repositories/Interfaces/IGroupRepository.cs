using DevEdu.DAL.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DevEdu.DAL.Repositories
{
    public interface IGroupRepository
    {
        Task<int> AddGroup(GroupDto groupDto);
        Task DeleteGroup(int id);
        Task<GroupDto> GetGroup(int id);
        Task<List<GroupDto>> GetGroups();
        Task<GroupDto> UpdateGroup(GroupDto groupDto);
        Task<int> AddUserToGroup(int groupId, int userId, int roleId);
        Task<int> DeleteUserFromGroup(int userId, int groupId);
        Task<int> AddGroupToLesson(int groupId, int lessonId);
        Task RemoveGroupFromLesson(int groupId, int lessonId);
        Task<GroupDto> ChangeGroupStatus(int groupId, int statusId);
        Task<int> AddGroupMaterialReference(int groupId, int materialId);
        Task<int> RemoveGroupMaterialReference(int groupId, int materialId);
        Task<int> AddTaskToGroup(GroupTaskDto groupTaskDto);
        Task DeleteTaskFromGroup(int groupId, int taskId);
        Task<List<GroupTaskDto>> GetTaskGroupByGroupId(int groupId);
        Task<GroupTaskDto> GetGroupTask(int groupId, int taskId);
        Task UpdateGroupTask(GroupTaskDto groupTaskDto);
        List<GroupDto> GetGroupsByMaterialId(int id);
        Task<int> GetPresentGroupForStudentByUserId(int userId);
    }
}