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

        public TaskDto AddTaskByMethodist(TaskDto taskDto, List<int> coursesIds, List<int> tagsIds, UserIdentityInfo userIdentityInfo)
        {
            var taskId = _taskRepository.AddTask(taskDto);
            if (tagsIds != null && tagsIds.Count != 0)
                AddTagsToTask(taskId, tagsIds, userIdentityInfo);
            var task = _taskRepository.GetTaskById(taskId);
            if (coursesIds != null && coursesIds.Count != 0)
                coursesIds.ForEach(courseId => _courseRepository.AddTaskToCourse(courseId, taskId));

            return task;
        }

        public async Task<TaskDto> AddTaskByTeacher(TaskDto taskDto, HomeworkDto homework, int groupId, List<int> tagsIds, UserIdentityInfo userIdentityInfo)
        {
            var taskId = _taskRepository.AddTask(taskDto);
            if (tagsIds != null && tagsIds.Count != 0)
                AddTagsToTask(taskId, tagsIds, userIdentityInfo);
            var task = _taskRepository.GetTaskById(taskId);
            if (homework != null)
            {
                homework.Group = await _groupRepository.GetGroup(groupId);
                homework.Task = task;
                _homeworkRepository.AddHomework(homework);
            }
            return task;
        }

        public TaskDto UpdateTask(TaskDto taskDto, int taskId, UserIdentityInfo userIdentityInfo)
        {
            _userValidationHelper.GetUserByIdAndThrowIfNotFound(userIdentityInfo.UserId);
            var task = _taskValidationHelper.GetTaskByIdAndThrowIfNotFound(taskId);
            if (userIdentityInfo.Roles.Contains(Role.Methodist) && !userIdentityInfo.Roles.Contains(Role.Admin))
            {
                _taskValidationHelper.CheckMethodistAccessToTask(task, userIdentityInfo.UserId);
            }
            if (userIdentityInfo.Roles.Contains(Role.Teacher) && !userIdentityInfo.Roles.Contains(Role.Admin))
                _taskValidationHelper.CheckUserAccessToTask(taskId, userIdentityInfo.UserId);

            taskDto.Id = taskId;
            _taskRepository.UpdateTask(taskDto);
            return _taskRepository.GetTaskById(taskId);
        }

        public int DeleteTask(int taskId, UserIdentityInfo userIdentityInfo)
        {
            _userValidationHelper.GetUserByIdAndThrowIfNotFound(userIdentityInfo.UserId);
            var task = _taskValidationHelper.GetTaskByIdAndThrowIfNotFound(taskId);
            if (userIdentityInfo.Roles.Contains(Role.Methodist) && !userIdentityInfo.Roles.Contains(Role.Admin))
            {
                _taskValidationHelper.CheckMethodistAccessToTask(task, userIdentityInfo.UserId);
            }
            if (userIdentityInfo.Roles.Contains(Role.Teacher) && !userIdentityInfo.Roles.Contains(Role.Admin))
                _taskValidationHelper.CheckUserAccessToTask(taskId, userIdentityInfo.UserId); 

            return _taskRepository.DeleteTask(taskId);
        }

        public TaskDto GetTaskById(int taskId, UserIdentityInfo userIdentityInfo)
        {
            _userValidationHelper.GetUserByIdAndThrowIfNotFound(userIdentityInfo.UserId);
            var task = _taskValidationHelper.GetTaskByIdAndThrowIfNotFound(taskId);
            AuthorizationException exception = default;
            bool authorized = true;
           
            if (userIdentityInfo.Roles.Contains(Role.Methodist) && !userIdentityInfo.Roles.Contains(Role.Admin))
            {
                var mException = _taskValidationHelper.CheckMethodistAccessToTask(task, userIdentityInfo.UserId);
                if (mException != default)
                {
                    exception = mException;
                    authorized = false;
                }
                else
                    return task;
            }
            if (!userIdentityInfo.Roles.Contains(Role.Admin) && !userIdentityInfo.Roles.Contains(Role.Methodist))
            {
                var uException = _taskValidationHelper.CheckUserAccessToTask(taskId, userIdentityInfo.UserId);
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

        public TaskDto GetTaskWithCoursesById(int taskId, UserIdentityInfo userIdentityInfo)
        {
            var taskDto = GetTaskById(taskId, userIdentityInfo);
            taskDto.Courses = _courseRepository.GetCoursesToTaskByTaskId(taskId);
            return taskDto;
        }

        public TaskDto GetTaskWithAnswersById(int taskId, UserIdentityInfo userIdentityInfo)
        {
            var taskDto = GetTaskById(taskId, userIdentityInfo);
            taskDto.StudentAnswers = _studentHomeworkRepository.GetAllStudentHomeworkByTask(taskId);
            return taskDto;
        }

        public TaskDto GetTaskWithGroupsById(int taskId, UserIdentityInfo userIdentityInfo)
        {
            var taskDto = GetTaskById(taskId, userIdentityInfo);
            taskDto.Groups = _groupRepository.GetGroupsByTaskId(taskId);
            return taskDto;
        }

        public List<TaskDto> GetTasks(UserIdentityInfo userIdentityInfo)
        {
            _userValidationHelper.GetUserByIdAndThrowIfNotFound(userIdentityInfo.UserId);
            var tasks = _taskRepository.GetTasks();
            var allowedTaskDtos = new List<TaskDto>();
            if (userIdentityInfo.Roles.Contains(Role.Admin))
                return tasks;
            if (userIdentityInfo.Roles.Contains(Role.Methodist))
            {
                allowedTaskDtos.AddRange(_taskValidationHelper.GetTasksAllowedToMethodist(tasks));
            }
            foreach (var task in tasks)
            {
                allowedTaskDtos.Add(_taskValidationHelper.GetTaskAllowedToUser(task.Id, userIdentityInfo.UserId));
            }
            return allowedTaskDtos;
        }

        public int AddTagToTask(int taskId, int tagId, UserIdentityInfo userIdentityInfo)
        {
            _userValidationHelper.GetUserByIdAndThrowIfNotFound(userIdentityInfo.UserId);
            var task = _taskValidationHelper.GetTaskByIdAndThrowIfNotFound(taskId);
            if (userIdentityInfo.Roles.Contains(Role.Methodist) && !userIdentityInfo.Roles.Contains(Role.Admin))
            {
                _taskValidationHelper.CheckMethodistAccessToTask(task, userIdentityInfo.UserId);
            }
            if (userIdentityInfo.Roles.Contains(Role.Teacher) || userIdentityInfo.Roles.Contains(Role.Tutor) && !userIdentityInfo.Roles.Contains(Role.Admin))
                _taskValidationHelper.CheckUserAccessToTask(taskId, userIdentityInfo.UserId);
         
            return _taskRepository.AddTagToTask(taskId, tagId);
        }

        public void AddTagsToTask(int taskId, List<int> tagsIds, UserIdentityInfo userIdentityInfo)
        {
            _userValidationHelper.GetUserByIdAndThrowIfNotFound(userIdentityInfo.UserId);
            var task = _taskValidationHelper.GetTaskByIdAndThrowIfNotFound(taskId);
            if (userIdentityInfo.Roles.Contains(Role.Methodist) && !userIdentityInfo.Roles.Contains(Role.Admin))
            {
                _taskValidationHelper.CheckMethodistAccessToTask(task, userIdentityInfo.UserId);
            }
            if (userIdentityInfo.Roles.Contains(Role.Teacher) || userIdentityInfo.Roles.Contains(Role.Tutor) && !userIdentityInfo.Roles.Contains(Role.Admin))
                _taskValidationHelper.CheckUserAccessToTask(taskId, userIdentityInfo.UserId);  

            tagsIds.ForEach(tagId => _taskRepository.AddTagToTask(taskId, tagId));
        }

        public int DeleteTagFromTask(int taskId, int tagId) => _taskRepository.DeleteTagFromTask(taskId, tagId);
    }
}