using DevEdu.Business.IdentityInfo;
using DevEdu.DAL.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DevEdu.Business.Services
{
    public interface ITaskService
    {
        Task<TaskDto> AddTaskByMethodistAsync(TaskDto taskDto, List<int> coursesIds, UserIdentityInfo userIdentityInfo);
        Task<TaskDto> AddTaskByTeacherAsync(TaskDto taskDto, HomeworkDto homework, int groupId, UserIdentityInfo userIdentityInfo);
        Task<int> DeleteTaskAsync(int taskId, UserIdentityInfo userIdentityInfo);
        Task<TaskDto> GetTaskByIdAsync(int taskId, UserIdentityInfo userIdentityInfo);
        Task<List<TaskDto>> GetTasksAsync(UserIdentityInfo userIdentityInfo);
        Task<TaskDto> GetTaskWithAnswersByIdAsync(int taskId, UserIdentityInfo userIdentityInfo);
        Task<TaskDto> GetTaskWithCoursesByIdAsync(int taskId, UserIdentityInfo userIdentityInfo);
        Task<TaskDto> GetTaskWithGroupsByIdAsync(int taskId, UserIdentityInfo userIdentityInfo);
        Task<TaskDto> UpdateTaskAsync(TaskDto taskDto, int taskId, UserIdentityInfo userIdentityInfo);
        Task<StudentHomeworkDto> GetStudentAnswerOnTaskAsync(int taskId, UserIdentityInfo userIdentityInfo);
    }
}