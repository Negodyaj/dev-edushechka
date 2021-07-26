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
        private readonly IGroupService _groupService;
        private readonly ICourseService _courseService;

        public TaskController(
            IMapper mapper, 
            ITaskService taskService,
            IStudentAnswerOnTaskService studentAnswerOnTaskService,
            ICommentRepository commentRepository,
            IGroupService groupService,
            ICourseService courseService
            )
        {
            _taskService = taskService;
            _mapper = mapper;
            _studentAnswerOnTaskService = studentAnswerOnTaskService;
            _commentRepository = commentRepository;
            _groupService = groupService;
            _courseService = courseService;
        }

        //  api/Task/1
        [AuthorizeRoles(Role.Methodist, Role.Teacher, Role.Tutor, Role.Student)]
        [HttpGet("{taskId}")]
        [Description("Get task by Id with tags")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public TaskInfoOutputModel GetTaskWithTags(int taskId)
        {
            var userId = this.GetUserId();
            var taskDto = _taskService.GetTaskById(taskId, userId);
            return _mapper.Map<TaskInfoOutputModel>(taskDto);
        }

        //  api/Task/1/with-courses
        [AuthorizeRoles(Role.Methodist)]
        [HttpGet("{taskId}/with-courses")]
        [Description("Get task by Id with tags and courses")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TaskInfoWithCoursesOutputModel))]
        public TaskInfoWithCoursesOutputModel GetTaskWithTagsAndCourses(int taskId)
        {
            var userId = this.GetUserId();
            var taskDto = _taskService.GetTaskWithCoursesById(taskId, userId);
            return _mapper.Map<TaskInfoWithCoursesOutputModel>(taskDto);
        }

        //  api/Task/1/with-answers
        [AuthorizeRoles(Role.Teacher, Role.Tutor)]
        [HttpGet("{taskId}/with-answers")]
        [Description("Get task by Id with tags and answers")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TaskInfoWithAnswersOutputModel))]
        public TaskInfoWithAnswersOutputModel GetTaskWithTagsAndAnswers(int taskId)
        {
            var userId = this.GetUserId();
            var taskDto = _taskService.GetTaskWithAnswersById(taskId, userId);
            return _mapper.Map<TaskInfoWithAnswersOutputModel>(taskDto);
        }

        //  api/Task/1/with-courses
        [AuthorizeRoles(Role.Teacher)]
        [HttpGet("{taskId}/with-groups")]
        [Description("Get task by Id with tags and groups")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TaskInfoWithCoursesOutputModel))]
        public TaskInfoWithGroupsOutputModel GetTaskWithTagsAndGroups(int taskId)
        {
            var userId = this.GetUserId();
            var taskDto = _taskService.GetTaskWithGroupsById(taskId, userId);
            return _mapper.Map<TaskInfoWithGroupsOutputModel>(taskDto);
        }

        //  api/Task
        [AuthorizeRoles(Role.Methodist, Role.Teacher, Role.Tutor, Role.Student)]
        [HttpGet]
        [Description("Get all tasks with tags")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TaskInfoOutputModel))]
        public List<TaskInfoOutputModel> GetAllTasksWithTags()
        {
            var userId = this.GetUserId();
            var taskDtos = _taskService.GetTasks(userId);
            return _mapper.Map<List<TaskInfoOutputModel>>(taskDtos);
        }

        // api/task/teacher
        [AuthorizeRoles(Role.Teacher)]
        [HttpPost("teacher")]
        [Description("Add new task by teacher")]
        [ProducesResponseType(StatusCodes.Status201Created)]
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
        [ProducesResponseType(StatusCodes.Status201Created)]
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
        [ProducesResponseType(StatusCodes.Status200OK)]
        public TaskInfoOutputModel UpdateTask(int taskId, [FromBody] TaskByTeacherInputModel model)
        {
            var userId = this.GetUserId();
            TaskDto taskDto = _mapper.Map<TaskDto>(model);
            return _mapper.Map<TaskInfoOutputModel>(_taskService.UpdateTask(taskDto, taskId, userId));
        }

        // api/task/{taskId}
        [AuthorizeRoles(Role.Methodist, Role.Teacher)]
        [HttpDelete("{taskId}")]
        [Description("Delete task with selected Id")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public void DeleteTask(int taskId)
        {
            var userId = this.GetUserId();
            _taskService.DeleteTask(taskId, userId);
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
        public void AddStudentAnswerOnTask(int taskId, int studentId, [FromBody] StudentAnswerOnTaskInputModel inputModel)
        {
            var taskAnswerDto = _mapper.Map<StudentAnswerOnTaskDto>(inputModel);
            _studentAnswerOnTaskService.AddStudentAnswerOnTask(taskId, studentId, taskAnswerDto);

        }

        // api/task/all-answers
        [HttpGet("all-answers")]
        public List<StudentAnswerOnTaskDto> GetAllStudentAnswersOnTasks()
        {
            var studentAnswerDto = _studentAnswerOnTaskService.GetAllStudentAnswersOnTasks();
            return studentAnswerDto;
        }

        // api/task/{taskId}/all-answers
        [HttpGet("{taskId}/all-answers")]
        public List<StudentAnswerOnTaskDto> GetAllStudentAnswersOnTask(int taskId)
        {
            var studentAnswerDto = _studentAnswerOnTaskService.GetAllStudentAnswersOnTask(taskId);
            return studentAnswerDto;
        }

        // api/task/{taskId}/student/{studentId}
        [HttpGet("{taskId}/student/{studentId}")]
        public StudentAnswerOnTaskDto GetStudentAnswerOnTaskByTaskIdAndStudentId(int taskId, int studentId)
        {
            var studentStatusDto = _studentAnswerOnTaskService.GetStudentAnswerOnTaskByTaskIdAndStudentId(taskId, studentId);
            return studentStatusDto;
        }


        // api/task/{taskId}/student/{studentId}
        [HttpPut("{taskId}/student/{studentId}")]
        public int UpdateStudentAnswerOnTask(int taskId, int studentId, [FromBody] StudentAnswerOnTaskInputModel inputModel)
        {
            var taskAnswerDto = _mapper.Map<StudentAnswerOnTaskDto>(inputModel);
            _studentAnswerOnTaskService.UpdateStudentAnswerOnTask(taskId, studentId, taskAnswerDto);

            return taskId;
        }

        // api/task/{taskId}/student/{studentId}
        [HttpDelete("{taskId}/student/{studentId}")]
        public string DeleteStudentAnswerOnTask(int taskId, int studentId)
        {
            _studentAnswerOnTaskService.DeleteStudentAnswerOnTask(taskId, studentId);

            return $"Deleted answer for task {taskId} id.";
        }

        // api/task/{taskId}/student/{studentId}/change-status/{statusId}
        [HttpPut("{taskId}/student/{studentId}/change-status/{statusId}")]
        public int UpdateStatusOfStudentAnswer(int taskId, int studentId, int statusId)
        {
            _studentAnswerOnTaskService.ChangeStatusOfStudentAnswerOnTask(taskId, studentId, statusId);

            return statusId;
        }

        // api/task/answer/{taskStudentId}/comment}
        [HttpPost("answer/{taskStudentId}/comment")]
        public int AddCommentOnStudentAnswer(int taskStudentId, [FromBody] CommentAddInputModel inputModel)
        {
            var commentDto = _mapper.Map<CommentDto>(inputModel);
            int commentId = _commentRepository.AddComment(commentDto);
            _studentAnswerOnTaskService.AddCommentOnStudentAnswer(taskStudentId, commentId);

            return taskStudentId;
        }

        //  api/task/1/group/
        [HttpGet("{taskId}/groups")]
        [Description("Get all groups by task")]
        [ProducesResponseType(typeof(List<GroupTaskInfoWithGroupOutputModel>), StatusCodes.Status200OK)]
        public List<GroupTaskInfoWithGroupOutputModel> GetGroupsByTaskId(int taskId)
        {
            var dto = _taskService.GetGroupTasksByTaskId(taskId);
            var output = _mapper.Map<List<GroupTaskInfoWithGroupOutputModel>>(dto);
            return output;
        }
    }
}