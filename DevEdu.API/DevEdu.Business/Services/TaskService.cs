using System.Collections.Generic;
using System.Linq;
using DevEdu.Business.Constants;
using DevEdu.Business.Exceptions;
using DevEdu.Business.ValidationHelpers;
using DevEdu.DAL.Enums;
using DevEdu.DAL.Models;
using DevEdu.DAL.Repositories;

namespace DevEdu.Business.Services
{
    public class TaskService : ITaskService
    {
        private readonly ITaskRepository _taskRepository;
        private readonly ICourseRepository _courseRepository;
        private readonly IStudentAnswerOnTaskRepository _studentAnswerOnTaskRepository;
        private readonly IGroupRepository _groupRepository;
        private readonly ITaskValidationHelper _taskValidationHelper;
        private readonly IUserValidationHelper _userValidationHelper;

        public TaskService(
            ITaskRepository taskRepository,
            ICourseRepository courseRepository,
            IStudentAnswerOnTaskRepository studentAnswerOnTaskRepository,
            IGroupRepository groupRepository,
            ITaskValidationHelper taskValidationHelper,
            IUserValidationHelper userValidationHelper
        )
        {
            _taskRepository = taskRepository;
            _courseRepository = courseRepository;
            _studentAnswerOnTaskRepository = studentAnswerOnTaskRepository;
            _groupRepository = groupRepository;
            _taskValidationHelper = taskValidationHelper;
            _userValidationHelper = userValidationHelper;
        }

        public TaskDto AddTaskByMethodist(TaskDto taskDto, List<int> coursesIds, List<int> tagsIds)
        {
            var taskId = _taskRepository.AddTask(taskDto);
            var task = _taskRepository.GetTaskById(taskId);
            if (tagsIds != null && tagsIds.Count != 0)
                tagsIds.ForEach(tagId => AddTagToTask(taskId, tagId));
            if (coursesIds != null && coursesIds.Count != 0)
                coursesIds.ForEach(courseId => _courseRepository.AddTaskToCourse(courseId, taskId));

            return task;
        }

        public TaskDto AddTaskByTeacher(TaskDto taskDto, GroupTaskDto groupTask, int groupId, List<int> tagsIds)
        {
            var taskId = _taskRepository.AddTask(taskDto);
            var task = _taskRepository.GetTaskById(taskId);
            if (tagsIds != null && tagsIds.Count != 0)
                tagsIds.ForEach(tagId => AddTagToTask(taskId, tagId));
            if (groupTask != null)
            {
                groupTask.Group = _groupRepository.GetGroup(groupId);
                groupTask.Task = task;
                _groupRepository.AddTaskToGroup(groupTask);
            }
            return task;
        }

        public TaskDto UpdateTask(TaskDto taskDto, int taskId, UserIdentityInfo userIdentityInfo)
        {
            _userValidationHelper.GetUserByIdAndThrowIfNotFound(userIdentityInfo.UserId);
            var task = _taskValidationHelper.GetTaskByIdAndThrowIfNotFound(taskId);
            if(userIdentityInfo.Roles.Contains(Role.Teacher) && !userIdentityInfo.Roles.Contains(Role.Admin))
                _taskValidationHelper.CheckUserAccessToTask(taskId, userIdentityInfo.UserId);
            if(userIdentityInfo.Roles.Contains(Role.Methodist) && !userIdentityInfo.Roles.Contains(Role.Admin))
            {
                _taskValidationHelper.CheckMethodistAccessToTask(task, userIdentityInfo.UserId);
            }

            taskDto.Id = taskId;
            _taskRepository.UpdateTask(taskDto);
            return _taskRepository.GetTaskById(taskId);
        }

        public int DeleteTask(int taskId, UserIdentityInfo userIdentityInfo)
        {
            _userValidationHelper.GetUserByIdAndThrowIfNotFound(userIdentityInfo.UserId);
            var task = _taskValidationHelper.GetTaskByIdAndThrowIfNotFound(taskId);
            if (userIdentityInfo.Roles.Contains(Role.Teacher) && !userIdentityInfo.Roles.Contains(Role.Admin))
                _taskValidationHelper.CheckUserAccessToTask(taskId, userIdentityInfo.UserId);
            if (userIdentityInfo.Roles.Contains(Role.Methodist) && !userIdentityInfo.Roles.Contains(Role.Admin))
            {
                _taskValidationHelper.CheckMethodistAccessToTask(task, userIdentityInfo.UserId);
            }

            return _taskRepository.DeleteTask(taskId);
        }

        public TaskDto GetTaskById(int taskid, UserIdentityInfo userIdentityInfo)
        {
            _userValidationHelper.GetUserByIdAndThrowIfNotFound(userIdentityInfo.UserId);
            var taskDto = _taskValidationHelper.GetTaskByIdAndThrowIfNotFound(taskid);
            if(!userIdentityInfo.Roles.Contains(Role.Admin))
            _taskValidationHelper.CheckUserAccessToTask(taskid, userIdentityInfo.UserId);

            return taskDto;
        }

        public TaskDto GetTaskWithCoursesById(int taskid, UserIdentityInfo userIdentityInfo)
        {
            var taskDto = GetTaskById(taskid, userIdentityInfo);
            taskDto.Courses = _courseRepository.GetCoursesToTaskByTaskId(taskid);
            return taskDto;
        }

        public TaskDto GetTaskWithAnswersById(int taskid, UserIdentityInfo userIdentityInfo)
        {
            var taskDto = GetTaskById(taskid, userIdentityInfo);
            taskDto.StudentAnswers = _studentAnswerOnTaskRepository.GetAllStudentAnswersOnTask(taskid);
            return taskDto;
        }

        public TaskDto GetTaskWithGroupsById(int taskid, UserIdentityInfo userIdentityInfo)
        {
            var taskDto = GetTaskById(taskid, userIdentityInfo);
            taskDto.Groups = _groupRepository.GetGroupsByTaskId(taskid);
            return taskDto;
        }

        public List<TaskDto> GetTasks(UserIdentityInfo userIdentityInfo)
        {
            _userValidationHelper.GetUserByIdAndThrowIfNotFound(userIdentityInfo.UserId);
            var tasks = _taskRepository.GetTasks();
            var allowedTaskDtos = new List<TaskDto>();
            if (!userIdentityInfo.Roles.Contains(Role.Admin))
                return tasks;
            foreach (var task in tasks)
            {
                allowedTaskDtos.Add(_taskValidationHelper.GetTaskAllowedToUser(task.Id, userIdentityInfo.UserId));
            }
            return allowedTaskDtos;
        }

        public int AddTagToTask(int taskId, int tagId)
        {
            return _taskRepository.AddTagToTask(taskId, tagId);
        }

        public int DeleteTagFromTask(int taskId, int tagId) => _taskRepository.DeleteTagFromTask(taskId, tagId);
    }
}