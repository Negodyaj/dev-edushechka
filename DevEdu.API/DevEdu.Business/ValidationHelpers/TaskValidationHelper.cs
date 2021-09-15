using DevEdu.Business.Constants;
using DevEdu.Business.Exceptions;
using DevEdu.DAL.Models;
using DevEdu.DAL.Repositories;
using System.Collections.Generic;
using System.Linq;

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

        public TaskDto GetTaskByIdAndThrowIfNotFound(int taskId)
        {
            var task = _taskRepository.GetTaskById(taskId);
            if (task == default)
                throw new EntityNotFoundException(string.Format(ServiceMessages.EntityNotFoundMessage, nameof(task), taskId));
            return task;
        }

        public AuthorizationException CheckUserAccessToTask(int taskId, int userId)
        {
            var groupsByTask = _groupRepository.GetGroupsByTaskId(taskId);
            var groupsByUser = _groupRepository.GetGroupsByUserId(userId);

            var result = groupsByTask.FirstOrDefault(gt => groupsByUser.Any(gu => gu.Id == gt.Id));
            if (result == default)
                return new AuthorizationException(string.Format(ServiceMessages.EntityDoesntHaveAcessMessage, "user", userId, "task", taskId));
            return default;
        }

        public AuthorizationException CheckMethodistAccessToTask(TaskDto taskDto, int userId)
        {
            taskDto.Courses = _courseRepository.GetCoursesToTaskByTaskId(taskDto.Id);
            if (taskDto.Courses == null)
                return new AuthorizationException(string.Format(ServiceMessages.EntityDoesntHaveAcessMessage, "user", userId, "task", taskDto.Id));
            return default;
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

        public List<TaskDto> GetTasksAllowedToMethodist(List<TaskDto> taskDtos)
        {
            return (List<TaskDto>)taskDtos.Where(t => t.Courses != null);
        }
    }
}