using DevEdu.Business.Constants;
using DevEdu.Business.Exceptions;
using DevEdu.DAL.Models;
using DevEdu.DAL.Repositories;
using System.Linq;

namespace DevEdu.Business.ValidationHelpers
{
    public class TaskValidationHelper : ITaskValidationHelper
    {
        private readonly ITaskRepository _taskRepository;
        private readonly IGroupRepository _groupRepository;

        public TaskValidationHelper(ITaskRepository taskRepository, IGroupRepository groupRepository)
        {
            _taskRepository = taskRepository;
            _groupRepository = groupRepository;
        }

        public TaskDto GetTaskByIdAndThrowIfNotFound(int taskId)
        {
            var task = _taskRepository.GetTaskById(taskId);
            if (task == default)
                throw new EntityNotFoundException(string.Format(ServiceMessages.EntityNotFoundMessage, nameof(task), taskId));
            return task;
        }

        public void CheckUserAccessToTask(int taskId, int userId)
        {
            var groupsByTask = _groupRepository.GetGroupsByTaskId(taskId);
            var groupsByUser = _groupRepository.GetGroupsByUserId(userId);

            var result = groupsByTask.FirstOrDefault(gt => groupsByUser.Any(gu => gu.Id == gt.Id));
            if (result == default)
                throw new AuthorizationException(string.Format(ServiceMessages.EntityDoesntHaveAccessMessage, "user", userId, "task", taskId));
        }

        public void CheckMethodistAccessToTask(TaskDto taskDto, int userId)
        {
            if (taskDto.Courses == null)
                throw new AuthorizationException(string.Format(ServiceMessages.EntityDoesntHaveAccessMessage, "user", userId, "task", taskDto.Id));
        }

        public TaskDto GetTaskAllowedToUser(int taskId, int userId)
        {
            var groupsByTask = _groupRepository.GetGroupsByTaskId(taskId);
            var groupsByUser = _groupRepository.GetGroupsByUserId(userId);

            var result = groupsByTask.FirstOrDefault(gt => groupsByUser.Any(gu => gu.Id == gt.Id));
            if (result == default)
                return null;
            return _taskRepository.GetTaskById(taskId);
        }
    }
}