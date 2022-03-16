using DevEdu.Business.Exceptions;
using DevEdu.Business.IdentityInfo;
using DevEdu.Business.ValidationHelpers;
using DevEdu.DAL.Enums;
using DevEdu.DAL.Models;
using DevEdu.DAL.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DevEdu.Business.Services
{
    public class TaskService : ITaskService
    {
        private readonly ITaskRepository _taskRepository;
        private readonly ICourseRepository _courseRepository;
        private readonly IStudentHomeworkRepository _studentHomeworkRepository;
        private readonly IGroupRepository _groupRepository;
        private readonly IHomeworkRepository _homeworkRepository;
        private readonly ITaskValidationHelper _taskValidationHelper;
        private readonly IUserValidationHelper _userValidationHelper;

        public TaskService(
            ITaskRepository taskRepository,
            ICourseRepository courseRepository,
            IStudentHomeworkRepository studentHomeworkRepository,
            IGroupRepository groupRepository,
            IHomeworkRepository homeworkRepository,
            ITaskValidationHelper taskValidationHelper,
            IUserValidationHelper userValidationHelper
        )
        {
            _taskRepository = taskRepository;
            _courseRepository = courseRepository;
            _studentHomeworkRepository = studentHomeworkRepository;
            _groupRepository = groupRepository;
            _homeworkRepository = homeworkRepository;
            _taskValidationHelper = taskValidationHelper;
            _userValidationHelper = userValidationHelper;
        }

        public async Task<TaskDto> AddTaskByMethodistAsync(TaskDto taskDto, List<int> coursesIds, UserIdentityInfo userIdentityInfo)
        {
            var taskId = await _taskRepository.AddTaskAsync(taskDto);

            var task = await _taskRepository.GetTaskByIdAsync(taskId);
            if (coursesIds != null && coursesIds.Count != 0)
                coursesIds.ForEach(courseId => _courseRepository.AddTaskToCourseAsync(courseId, taskId));

            return task;
        }

        public async Task<TaskDto> AddTaskByTeacherAsync(TaskDto taskDto, HomeworkDto homework, int groupId, UserIdentityInfo userIdentityInfo)
        {
            var taskId = await _taskRepository.AddTaskAsync(taskDto);

            var task = await _taskRepository.GetTaskByIdAsync(taskId);
            if (homework != null)
            {
                homework.Group = await _groupRepository.GetGroupAsync(groupId);
                homework.Task = task;
                await _homeworkRepository.AddHomeworkAsync(homework);
            }

            return task;
        }

        public async Task<TaskDto> UpdateTaskAsync(TaskDto taskDto, int taskId, UserIdentityInfo userIdentityInfo)
        {
            await _userValidationHelper.GetUserByIdAndThrowIfNotFoundAsync(userIdentityInfo.UserId);
            var task = await _taskValidationHelper.GetTaskByIdAndThrowIfNotFoundAsync(taskId);
            AuthorizationException exception = default;
            bool authorized = true;

            if (userIdentityInfo.Roles.Contains(Role.Methodist) &&
                !userIdentityInfo.Roles.Contains(Role.Admin))
            {
                var mException = await _taskValidationHelper.CheckMethodistAccessToTaskAsync(task, userIdentityInfo.UserId);
                if (mException != default)
                {
                    exception = mException;
                    authorized = false;
                }
                else
                {
                    taskDto.Id = taskId;
                    await _taskRepository.UpdateTaskAsync(taskDto);
                    return await _taskRepository.GetTaskByIdAsync(taskId);
                }
            }
            if (userIdentityInfo.Roles.Contains(Role.Teacher) &&
                !userIdentityInfo.Roles.Contains(Role.Admin))
            {
                var uException = await _taskValidationHelper.CheckUserAccessToTaskAsync(taskId, userIdentityInfo.UserId);
                if (uException != default)
                {
                    exception = uException;
                    authorized = false;
                }
                else
                {
                    taskDto.Id = taskId;
                    await _taskRepository.UpdateTaskAsync(taskDto);
                    return await _taskRepository.GetTaskByIdAsync(taskId);
                }
            }

            if (!authorized)
                throw exception;

            taskDto.Id = taskId;
            await _taskRepository.UpdateTaskAsync(taskDto);
            return await _taskRepository.GetTaskByIdAsync(taskId);
        }

        public async Task<int> DeleteTaskAsync(int taskId, UserIdentityInfo userIdentityInfo)
        {
            await _userValidationHelper.GetUserByIdAndThrowIfNotFoundAsync(userIdentityInfo.UserId);
            var task = await _taskValidationHelper.GetTaskByIdAndThrowIfNotFoundAsync(taskId);
            AuthorizationException exception = default;
            bool authorized = true;

            if (userIdentityInfo.Roles.Contains(Role.Methodist) &&
                !userIdentityInfo.Roles.Contains(Role.Admin))
            {
                var mException = await _taskValidationHelper.CheckMethodistAccessToTaskAsync(task, userIdentityInfo.UserId);
                if (mException != default)
                {
                    exception = mException;
                    authorized = false;
                }
                else
                    return await _taskRepository.DeleteTaskAsync(taskId);
            }
            if (userIdentityInfo.Roles.Contains(Role.Teacher) &&
                !userIdentityInfo.Roles.Contains(Role.Admin))
            {
                var uException = await _taskValidationHelper.CheckUserAccessToTaskAsync(taskId, userIdentityInfo.UserId);
                if (uException != default)
                {
                    exception = uException;
                    authorized = false;
                }
                else
                    return await _taskRepository.DeleteTaskAsync(taskId);
            }

            if (!authorized)
                throw exception;

            return await _taskRepository.DeleteTaskAsync(taskId);
        }

        public async Task<TaskDto> GetTaskByIdAsync(int taskId, UserIdentityInfo userIdentityInfo)
        {
            await _userValidationHelper.GetUserByIdAndThrowIfNotFoundAsync(userIdentityInfo.UserId);
            var task = await _taskValidationHelper.GetTaskByIdAndThrowIfNotFoundAsync(taskId);
            AuthorizationException exception = default;
            bool authorized = true;

            if (userIdentityInfo.Roles.Contains(Role.Methodist) &&
                !userIdentityInfo.Roles.Contains(Role.Admin))
            {
                var mException = await _taskValidationHelper.CheckMethodistAccessToTaskAsync(task, userIdentityInfo.UserId);
                if (mException != default)
                {
                    exception = mException;
                    authorized = false;
                }
                else
                    return task;
            }
            if (!userIdentityInfo.Roles.Contains(Role.Admin) &&
                !userIdentityInfo.Roles.Contains(Role.Methodist))
            {
                var uException = await _taskValidationHelper.CheckUserAccessToTaskAsync(taskId, userIdentityInfo.UserId);
                if (uException != default)
                {
                    exception = uException;
                    authorized = false;
                }
                else
                    return task;
            }

            if (!authorized)
                throw exception;

            return task;
        }

        public async Task<TaskDto> GetTaskWithCoursesByIdAsync(int taskId, UserIdentityInfo userIdentityInfo)
        {
            var taskDto = await GetTaskByIdAsync(taskId, userIdentityInfo);
            taskDto.Courses = await _courseRepository.GetCoursesToTaskByTaskIdAsync(taskId);
            return taskDto;
        }

        public async Task<TaskDto> GetTaskWithAnswersByIdAsync(int taskId, UserIdentityInfo userIdentityInfo)
        {
            var taskDto = await GetTaskByIdAsync(taskId, userIdentityInfo);
            taskDto.StudentAnswers = await _studentHomeworkRepository.GetAllStudentHomeworkByTaskAsync(taskId);
            return taskDto;
        }

        public async Task<TaskDto> GetTaskWithGroupsByIdAsync(int taskId, UserIdentityInfo userIdentityInfo)
        {
            var taskDto = await GetTaskByIdAsync(taskId, userIdentityInfo);
            taskDto.Groups = await _groupRepository.GetGroupsByTaskIdAsync(taskId);
            return taskDto;
        }

        public async Task<List<TaskDto>> GetTasksAsync(UserIdentityInfo userIdentityInfo)
        {
            await _userValidationHelper.GetUserByIdAndThrowIfNotFoundAsync(userIdentityInfo.UserId);
            var tasks = await _taskRepository.GetTasksAsync();
            var allowedTaskDtos = new List<TaskDto>();

            if (userIdentityInfo.Roles.Contains(Role.Admin))
                return tasks;

            if (userIdentityInfo.Roles.Contains(Role.Methodist))
            {
                allowedTaskDtos.AddRange(_taskValidationHelper.GetTasksAllowedToMethodist(tasks));
            }
            foreach (var task in tasks)
            {
                var allowedTask = _taskValidationHelper.GetTaskAllowedToUserAsync(task.Id, userIdentityInfo.UserId).Result;
                if (allowedTask != null)
                    allowedTaskDtos.Add(allowedTask);
            }

            return allowedTaskDtos;
        }
    }
}