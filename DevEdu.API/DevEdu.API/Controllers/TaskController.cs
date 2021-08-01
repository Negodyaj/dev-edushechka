using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using DevEdu.API.Models.InputModels;
using System.Collections.Generic;
using System.ComponentModel;
using DevEdu.API.Common;
using DevEdu.API.Extensions;
using DevEdu.API.Models.OutputModels;
using DevEdu.DAL.Repositories;
using DevEdu.DAL.Models;
using DevEdu.Business.Services;
using DevEdu.DAL.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using DevEdu.API.Configuration.ExceptionResponses;

namespace DevEdu.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class TaskController : Controller
    {
        private readonly IMapper _mapper;
        private readonly ITaskService _taskService;
        private readonly IStudentAnswerOnTaskService _studentAnswerOnTaskService;
        private readonly ICommentRepository _commentRepository;
        private readonly ICommentService _commentService;
        private readonly IUserService _userService;
        private readonly IGroupService _groupService;
        private readonly ICourseService _courseService;

        public TaskController(
            IMapper mapper,
            ITaskService taskService,
            IStudentAnswerOnTaskService studentAnswerOnTaskService,
            ICommentRepository commentRepository,
            IGroupService groupService,
            ICourseService courseService,
            ICommentService commentService)
        {
            _taskService = taskService;
            _mapper = mapper;
            _studentAnswerOnTaskService = studentAnswerOnTaskService;
            _commentRepository = commentRepository;
            _groupService = groupService;
            _courseService = courseService;
            _commentService = commentService;
        }

        // api/task/teacher
        [AuthorizeRoles(Role.Teacher)]
        [HttpPost("teacher")]
        [Description("Add new task by teacher")]
        [ProducesResponseType(typeof(TaskInfoOutputModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status403Forbidden)]
        public TaskInfoOutputModel AddTaskByTeacher([FromBody] TaskByTeacherInputModel model)
        {
            var taskDto = _mapper.Map<TaskDto>(model);
            var groupTaskDto = _mapper.Map<GroupTaskDto>(model.GroupTask);
            var task = _taskService.AddTaskByTeacher(taskDto, groupTaskDto, model.GroupId, model.Tags);

            return _mapper.Map<TaskInfoOutputModel>(task);
        }

        // api/task/methodist
        [AuthorizeRoles(Role.Methodist)]
        [HttpPost("methodist")]
        [Description("Add new task by methodist")]
        [ProducesResponseType(typeof(TaskInfoOutputModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status403Forbidden)]
        public TaskInfoOutputModel AddTaskByMethodist([FromBody] TaskByMethodistInputModel model)
        {
            var taskDto = _mapper.Map<TaskDto>(model);
            var task = _taskService.AddTaskByMethodist(taskDto, model.CoursesIds, model.Tags);

            return _mapper.Map<TaskInfoOutputModel>(task);
        }

        // api/task/{taskId}
        [AuthorizeRoles(Role.Methodist, Role.Teacher)]
        [HttpPut("{taskId}")]
        [Description("Update task")]
        [ProducesResponseType(typeof(TaskInfoOutputModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ValidationExceptionResponse), StatusCodes.Status422UnprocessableEntity)]
        public TaskInfoOutputModel UpdateTask(int taskId, [FromBody] TaskByTeacherInputModel model)
        {
            var userId = this.GetUserId();
            var roles = this.GetUserRoles();
            TaskDto taskDto = _mapper.Map<TaskDto>(model);
            return _mapper.Map<TaskInfoOutputModel>(_taskService.UpdateTask(taskDto, taskId, userId, roles));
        }

        // api/task/{taskId}
        [AuthorizeRoles(Role.Methodist, Role.Teacher)]
        [HttpDelete("{taskId}")]
        [Description("Delete task with selected Id")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ValidationExceptionResponse), StatusCodes.Status422UnprocessableEntity)]
        public void DeleteTask(int taskId)
        {
            var roles = this.GetUserRoles();
            var userId = this.GetUserId();
            _taskService.DeleteTask(taskId, userId, roles);
        }


        //  api/Task/1
        [AuthorizeRoles(Role.Methodist, Role.Teacher, Role.Tutor, Role.Student)]
        [HttpGet("{taskId}")]
        [Description("Get task by Id with tags")]
        [ProducesResponseType(typeof(TaskInfoOutputModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ValidationExceptionResponse), StatusCodes.Status422UnprocessableEntity)]
        public TaskInfoOutputModel GetTaskWithTags(int taskId)
        {
            var userId = this.GetUserId();
            var roles = this.GetUserRoles();
            bool isAdmin = roles.Contains(Role.Admin);
            var taskDto = _taskService.GetTaskById(taskId, userId, isAdmin);
            return _mapper.Map<TaskInfoOutputModel>(taskDto);
        }

        //  api/Task/1/with-courses
        [AuthorizeRoles(Role.Methodist)]
        [HttpGet("{taskId}/with-courses")]
        [Description("Get task by Id with tags and courses")]
        [ProducesResponseType(typeof(TaskInfoWithCoursesOutputModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ValidationExceptionResponse), StatusCodes.Status422UnprocessableEntity)]
        public TaskInfoWithCoursesOutputModel GetTaskWithTagsAndCourses(int taskId)
        {
            var userId = this.GetUserId();
            var roles = this.GetUserRoles();
            bool isAdmin = roles.Contains(Role.Admin);
            var taskDto = _taskService.GetTaskWithCoursesById(taskId, userId, isAdmin);
            return _mapper.Map<TaskInfoWithCoursesOutputModel>(taskDto);
        }

        //  api/Task/1/with-answers
        [AuthorizeRoles(Role.Teacher, Role.Tutor)]
        [HttpGet("{taskId}/with-answers")]
        [Description("Get task by Id with tags and answers")]
        [ProducesResponseType(typeof(TaskInfoWithAnswersOutputModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ValidationExceptionResponse), StatusCodes.Status422UnprocessableEntity)]
        public TaskInfoWithAnswersOutputModel GetTaskWithTagsAndAnswers(int taskId)
        {
            var userId = this.GetUserId();
            var roles = this.GetUserRoles();
            bool isAdmin = roles.Contains(Role.Admin);
            var taskDto = _taskService.GetTaskWithAnswersById(taskId, userId, isAdmin);
            return _mapper.Map<TaskInfoWithAnswersOutputModel>(taskDto);
        }

        //  api/Task/1/with-courses
        [AuthorizeRoles(Role.Teacher)]
        [HttpGet("{taskId}/with-groups")]
        [Description("Get task by Id with tags and groups")]
        [ProducesResponseType(typeof(TaskInfoWithGroupsOutputModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ValidationExceptionResponse), StatusCodes.Status422UnprocessableEntity)]
        public TaskInfoWithGroupsOutputModel GetTaskWithTagsAndGroups(int taskId)
        {
            var userId = this.GetUserId();
            var roles = this.GetUserRoles();
            bool isAdmin = roles.Contains(Role.Admin);
            var taskDto = _taskService.GetTaskWithGroupsById(taskId, userId, isAdmin);
            return _mapper.Map<TaskInfoWithGroupsOutputModel>(taskDto);
        }

        //  api/Task
        [AuthorizeRoles(Role.Methodist, Role.Teacher, Role.Tutor, Role.Student)]
        [HttpGet]
        [Description("Get all tasks with tags")]
        [ProducesResponseType(typeof(List<TaskInfoOutputModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ValidationExceptionResponse), StatusCodes.Status422UnprocessableEntity)]
        public List<TaskInfoOutputModel> GetAllTasksWithTags()
        {
            var userId = this.GetUserId();
            var roles = this.GetUserRoles();
            bool isAdmin = roles.Contains(Role.Admin);
            var taskDtos = _taskService.GetTasks(userId, isAdmin);
            return _mapper.Map<List<TaskInfoOutputModel>>(taskDtos);
        }

        // api/task/{taskId}/tag/{tagId}
        [HttpPost("{taskId}/tag/{tagId}")]
        [Description("Add tag to task")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public void AddTagToTask(int taskId, int tagId)
        {
            _taskService.AddTagToTask(taskId, tagId);
        }

        // api/task/{taskId}/tag/{tagId}
        [HttpDelete("{taskId}/tag/{tagId}")]
        [Description("Delete tag from task")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public void DeleteTagFromTask(int taskId, int tagId)
        {
            _taskService.DeleteTagFromTask(taskId, tagId);
        }

        // api/task/{taskId}/student/{studentId}
        [HttpPost("{taskId}/student/{studentId}")]
        [Description("Add student answer on task")]
        [ProducesResponseType(typeof(StudentAnswerOnTaskFullOutputModel), StatusCodes.Status201Created)]
        public StudentAnswerOnTaskFullOutputModel AddStudentAnswerOnTask(int taskId, int studentId, [FromBody] StudentAnswerOnTaskInputModel inputModel)
        {
            var taskAnswerDto = _mapper.Map<StudentAnswerOnTaskDto>(inputModel);
            _studentAnswerOnTaskService.AddStudentAnswerOnTask(taskId, studentId, taskAnswerDto);
            var studentAnswerDto = _studentAnswerOnTaskService.GetStudentAnswerOnTaskByTaskIdAndStudentId(taskId, studentId);
            var output = _mapper.Map<StudentAnswerOnTaskFullOutputModel>(studentAnswerDto);

            return output;
        }

        // api/task/{taskId}/all-answers
        [HttpGet("{taskId}/all-answers")]
        [Description("Get all student answers on tasks by task")]
        [ProducesResponseType(typeof(List<StudentAnswerOnTaskFullOutputModel>), StatusCodes.Status200OK)]
        public List<StudentAnswerOnTaskFullOutputModel> GetAllStudentAnswersOnTask(int taskId)
        {
            var studentAnswersDto = _studentAnswerOnTaskService.GetAllStudentAnswersOnTask(taskId);
            var output = _mapper.Map<List<StudentAnswerOnTaskFullOutputModel>>(studentAnswersDto);

            return output;
        }

        // api/task/{taskId}/student/{studentId}
        [HttpGet("{taskId}/student/{studentId}")]
        [Description("Get student answers on tasks by student and task")]
        [ProducesResponseType(typeof(StudentAnswerOnTaskFullOutputModel), StatusCodes.Status200OK)]
        public StudentAnswerOnTaskFullOutputModel GetStudentAnswerOnTaskByTaskIdAndStudentId(int taskId, int studentId)
        {
            var studentAnswerDto = _studentAnswerOnTaskService.GetStudentAnswerOnTaskByTaskIdAndStudentId(taskId, studentId);
            var output = _mapper.Map<StudentAnswerOnTaskFullOutputModel>(studentAnswerDto);

            return output;
        }


        // api/task/{taskId}/student/{studentId}
        [HttpPut("{taskId}/student/{studentId}")]
        [Description("Update student answer on task")]
        [ProducesResponseType(typeof(StudentAnswerOnTaskFullOutputModel), StatusCodes.Status200OK)]
        public StudentAnswerOnTaskFullOutputModel UpdateStudentAnswerOnTask(int taskId, int studentId, [FromBody] StudentAnswerOnTaskInputModel inputModel)
        {
            var taskAnswerDto = _mapper.Map<StudentAnswerOnTaskDto>(inputModel);
            _studentAnswerOnTaskService.UpdateStudentAnswerOnTask(taskId, studentId, taskAnswerDto);
            var output = _studentAnswerOnTaskService.GetStudentAnswerOnTaskByTaskIdAndStudentId(taskId, studentId);

            return _mapper.Map<StudentAnswerOnTaskFullOutputModel>(output);
        }

        // api/task/{taskId}/student/{studentId}
        [HttpDelete("{taskId}/student/{studentId}")]
        [Description("Delete student answer on task")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public void DeleteStudentAnswerOnTask(int taskId, int studentId)
        {
            _studentAnswerOnTaskService.DeleteStudentAnswerOnTask(taskId, studentId);
        }

        // api/task/{taskId}/student/{studentId}/change-status/{statusId}
        [HttpPut("{taskId}/student/{studentId}/change-status/{statusId}")]
        [Description("Update task status of student answer")]
        [ProducesResponseType(typeof(StudentAnswerOnTaskFullOutputModel), StatusCodes.Status200OK)]
        public StudentAnswerOnTaskFullOutputModel UpdateStatusOfStudentAnswer(int taskId, int studentId, int statusId)
        {
            _studentAnswerOnTaskService.ChangeStatusOfStudentAnswerOnTask(taskId, studentId, statusId);
            var output = _studentAnswerOnTaskService.GetStudentAnswerOnTaskByTaskIdAndStudentId(taskId, studentId);

            return _mapper.Map<StudentAnswerOnTaskFullOutputModel>(output);
        }

        // api/task/answer/{taskStudentId}/comment}
        [HttpPost("answer/{taskStudentId}/comment")]
        [Description("Add comment on task student answer")]
        [ProducesResponseType(typeof(CommentInfoOutputModel), StatusCodes.Status204NoContent)]
        public CommentInfoOutputModel AddCommentOnStudentAnswer(int taskStudentId, [FromBody] CommentAddInputModel inputModel)
        {
            var commentDto = _mapper.Map<CommentDto>(inputModel);
            int commentId = _commentService.AddComment(commentDto);
            _studentAnswerOnTaskService.AddCommentOnStudentAnswer(taskStudentId, commentId);

            var output = _commentService.GetComment(commentId);
            return _mapper.Map<CommentInfoOutputModel>(output);
        }
        // api/task/answer/by-user/42
        [HttpGet("answer/by-user/{userId}")]
        [Description("Get all answers of student")]
        [ProducesResponseType(typeof(List<StudentAnswerOnTaskOutputModel>), StatusCodes.Status200OK)]
        public List<StudentAnswerOnTaskOutputModel> GetAllAnswersByStudentId(int userId)
        {
            var answersDto = _studentAnswerOnTaskService.GetAllAnswersByStudentId(userId);
            var output = _mapper.Map<List<StudentAnswerOnTaskOutputModel>>(answersDto);

            return output;
        }
    }
}