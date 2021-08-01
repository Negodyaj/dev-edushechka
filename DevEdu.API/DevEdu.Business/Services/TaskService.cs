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
            if (tagsIds != null || tagsIds.Count != 0)
                tagsIds.ForEach(tagId => AddTagToTask(taskId, tagId));
            if (coursesIds != null || coursesIds.Count != 0)
                coursesIds.ForEach(courseId => _courseRepository.AddTaskToCourse(courseId, taskId));

            return task;
        }

        public TaskDto AddTaskByTeacher(TaskDto taskDto, GroupTaskDto groupTask, int groupId, List<int> tagsIds)
        {
            var taskId = _taskRepository.AddTask(taskDto);
            var task = _taskRepository.GetTaskById(taskId);
            if (tagsIds != null || tagsIds.Count != 0)
                tagsIds.ForEach(tagId => AddTagToTask(taskId, tagId));
            if (groupTask != null)
            {
                groupTask.Group = _groupRepository.GetGroup(groupId);
                groupTask.Task = task;
                _groupRepository.AddTaskToGroup(groupTask);
            }
            return task;
        }

        public TaskDto UpdateTask(TaskDto taskDto, int taskId, int userId, List<Role> roles)
        {
            _userValidationHelper.CheckUserExistence(userId);
            var task = _taskValidationHelper.GetTaskByIdAndThrowIfNotFound(taskId);
            if(roles.Contains(Role.Teacher) || !roles.Contains(Role.Admin))
                _taskValidationHelper.CheckUserAccessToTask(taskId, userId);
            if(roles.Contains(Role.Methodist) || !roles.Contains(Role.Admin))
            {
                _taskValidationHelper.CheckMethodistAccessToTask(task, userId);
            }

            taskDto.Id = taskId;
            _taskRepository.UpdateTask(taskDto);
            return _taskRepository.GetTaskById(taskId);
        }

        public void DeleteTask(int taskId, int userId, List<Role> roles)
        {
            _userValidationHelper.CheckUserExistence(userId);
            var task = _taskValidationHelper.GetTaskByIdAndThrowIfNotFound(taskId);
            if (roles.Contains(Role.Teacher) || !roles.Contains(Role.Admin))
                _taskValidationHelper.CheckUserAccessToTask(taskId, userId);
            if (roles.Contains(Role.Methodist) || !roles.Contains(Role.Admin))
            {
                _taskValidationHelper.CheckMethodistAccessToTask(task, userId);
            }

            _taskRepository.DeleteTask(taskId);
        }

        public TaskDto GetTaskById(int taskid, int userId, bool isAdmin)
        {
            _userValidationHelper.CheckUserExistence(userId);
            var taskDto = _taskValidationHelper.GetTaskByIdAndThrowIfNotFound(taskid);
            if(!isAdmin)
            _taskValidationHelper.CheckUserAccessToTask(taskid, userId);

            return taskDto;
        }

        public TaskDto GetTaskWithCoursesById(int taskid, int userId, bool isAdmin)
        {
            var taskDto = GetTaskById(taskid, userId, isAdmin);
            taskDto.Courses = _courseRepository.GetCoursesToTaskByTaskId(taskid);
            return taskDto;
        }

        public TaskDto GetTaskWithAnswersById(int taskid, int userId, bool isAdmin)
        {
            var taskDto = GetTaskById(taskid, userId, isAdmin);
            taskDto.StudentAnswers = _studentAnswerOnTaskRepository.GetStudentAnswersToTaskByTaskId(taskid);
            return taskDto;
        }

        public TaskDto GetTaskWithGroupsById(int taskid, int userId, bool isAdmin)
        {
            var taskDto = GetTaskById(taskid, userId, isAdmin);
            taskDto.Groups = _groupRepository.GetGroupsByTaskId(taskid);
            return taskDto;
        }

        public List<TaskDto> GetTasks(int userId, bool isAdmin)
        {
            _userValidationHelper.CheckUserExistence(userId);
            var tasks = _taskRepository.GetTasks();
            var allowedTaskDtos = new List<TaskDto>();
            if (!isAdmin)
                return tasks;
            foreach (var task in tasks)
            {
                allowedTaskDtos.Add(_taskValidationHelper.GetTaskAllowedToUser(task.Id, userId));
            }
            return allowedTaskDtos;
        }

        public int AddTagToTask(int taskId, int tagId)
        {
            return _taskRepository.AddTagToTask(taskId, tagId);
        }

        public void DeleteTagFromTask(int taskId, int tagId) => _taskRepository.DeleteTagFromTask(taskId, tagId);
    }
}