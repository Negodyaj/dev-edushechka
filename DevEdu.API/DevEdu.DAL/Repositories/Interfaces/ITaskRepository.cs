using DevEdu.DAL.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DevEdu.DAL.Repositories
{
    public interface ITaskRepository
    {
        Task<int> AddTaskAsync(TaskDto task);
        Task UpdateTaskAsync(TaskDto task);
        Task<int> DeleteTaskAsync(int id);
        Task<TaskDto> GetTaskByIdAsync(int id);
        Task<List<TaskDto>> GetTasksAsync();
        Task<List<TaskDto>> GetTasksByCourseIdAsync(int courseId);
    }
}