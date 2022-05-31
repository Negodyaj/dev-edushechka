using DevEdu.Business.Exceptions;
using DevEdu.DAL.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DevEdu.Business.ValidationHelpers
{
    public interface ITaskValidationHelper
    {
        Task<TaskDto> GetTaskByIdAndThrowIfNotFoundAsync(int taskId);
        Task<AuthorizationException> CheckUserAccessToTaskAsync(TaskDto taskDto, int userId);
        Task<AuthorizationException> CheckMethodistAccessToTaskAsync(TaskDto taskDto, int userId);
        Task<TaskDto> GetTaskAllowedToUserAsync(int taskId, int userId);
        List<TaskDto> GetTasksAllowedToMethodist(List<TaskDto> taskDtos);
    }
}