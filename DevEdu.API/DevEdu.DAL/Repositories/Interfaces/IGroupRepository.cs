using DevEdu.DAL.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DevEdu.DAL.Repositories
{
    public interface IGroupRepository
    {
        Task<int> AddGroupAsync(GroupDto groupDto);
        Task DeleteGroupAsync(int id);
        Task<GroupDto> GetGroupAsync(int id);
        Task<List<GroupDto>> GetGroupsAsync();
        Task<GroupDto> UpdateGroupAsync(GroupDto groupDto);
        Task<int> AddUserToGroupAsync(int groupId, int userId, int roleId);
        Task<int> DeleteUserFromGroupAsync(int userId, int groupId);
        Task<int> AddGroupToLessonAsync(int groupId, int lessonId);
        Task RemoveGroupFromLessonAsync(int groupId, int lessonId);
        Task<GroupDto> ChangeGroupStatusAsync(int groupId, int statusId);
        Task<int> AddGroupMaterialReferenceAsync(int groupId, int materialId);
        Task<int> RemoveGroupMaterialReferenceAsync(int groupId, int materialId);
        Task DeleteTaskFromGroupAsync(int groupId, int taskId);
        Task<List<GroupDto>> GetGroupsByMaterialIdAsync(int id);
        Task<int> GetPresentGroupForStudentByUserIdAsync(int userId);
        Task<List<GroupDto>> GetGroupsByTaskIdAsync(int taskId);
        Task<List<GroupDto>> GetGroupsByLessonIdAsync(int lessonId);
        Task<List<GroupDto>> GetGroupsByCourseIdAsync(int courseId);
        Task<List<GroupDto>> GetGroupsByUserIdAsync(int userId);
    }
}