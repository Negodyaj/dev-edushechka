using DevEdu.Business.Constants;
using DevEdu.Business.Exceptions;
using DevEdu.DAL.Models;
using DevEdu.DAL.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DevEdu.Business.ValidationHelpers
{
    public class TaskValidationHelper : ITaskValidationHelper
    {
        private readonly ITaskRepository _taskRepository;
        private readonly IGroupRepository _groupRepository;
        private readonly ICourseRepository _courseRepository;

        public TaskValidationHelper(ITaskRepository taskRepository, IGroupRepository groupRepository, ICourseRepository courseRepository)
        {
            _taskRepository = taskRepository;
            _groupRepository = groupRepository;
            _courseRepository = courseRepository;
        }

        public async Task<TaskDto> GetTaskByIdAndThrowIfNotFoundAsync(int taskId)
        {
            var task = await _taskRepository.GetTaskByIdAsync(taskId);
            if (task == default)
                throw new EntityNotFoundException(string.Format(ServiceMessages.EntityNotFoundMessage, nameof(task), taskId));
            return task;
        }

        public async Task<AuthorizationException> CheckUserAccessToTaskAsync(int taskId, int userId)
        {
            var groupsByTask = await _groupRepository.GetGroupsByTaskIdAsync(taskId);
            var groupsByUser = await _groupRepository.GetGroupsByUserIdAsync(userId);

            var result = groupsByTask.FirstOrDefault(gt => groupsByUser.Any(gu => gu.Id == gt.Id));
            if (result == default)
                return new AuthorizationException(string.Format(ServiceMessages.EntityDoesntHaveAcessMessage, "user", userId, "task", taskId));
            return default;
        }

        public async Task<AuthorizationException> CheckMethodistAccessToTaskAsync(TaskDto taskDto, int userId)
        {
            taskDto.Courses = await _courseRepository.GetCoursesToTaskByTaskIdAsync(taskDto.Id);
            if (taskDto.Courses == null)
                return new AuthorizationException(string.Format(ServiceMessages.EntityDoesntHaveAcessMessage, "user", userId, "task", taskDto.Id));
            return default;
        }

        public async Task<TaskDto> GetTaskAllowedToUserAsync(int taskId, int userId)
        {
            var groupsByTask = await _groupRepository.GetGroupsByTaskIdAsync(taskId);
            var groupsByUser = await _groupRepository.GetGroupsByUserIdAsync(userId);

            var result = groupsByTask.FirstOrDefault(gt => groupsByUser.Any(gu => gu.Id == gt.Id));
            if (result == default)
                return null;
            return await _taskRepository.GetTaskByIdAsync(taskId);
        }

        public List<TaskDto> GetTasksAllowedToMethodist(List<TaskDto> taskDtos)
        {
            return (List<TaskDto>)taskDtos.Where(t => t.Courses != null);
        }
    }
}