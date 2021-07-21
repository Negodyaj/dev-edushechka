using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using DevEdu.API.Models.InputModels;
using System.Collections.Generic;
using System.ComponentModel;
using DevEdu.API.Models.OutputModels;
using DevEdu.DAL.Repositories;
using DevEdu.DAL.Models;
using DevEdu.Business.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;

namespace DevEdu.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TaskController : Controller
    {
        private readonly IMapper _mapper;
        private readonly ITaskService _taskService;
        private readonly IStudentAnswerOnTaskService _studentAnswerOnTaskService;
        private readonly ITaskRepository _taskRepository;
        private readonly ICommentRepository _commentRepository;

        public TaskController(
            IMapper mapper, 
            ITaskService taskService,
            IStudentAnswerOnTaskService studentAnswerOnTaskService, 
            ITaskRepository taskRepository,
            ICommentRepository commentRepository)
        {
            _taskService = taskService;
            _mapper = mapper;
            _studentAnswerOnTaskService = studentAnswerOnTaskService;
            _taskRepository = taskRepository;
            _commentRepository = commentRepository;
        }

        //  api/Task/1
        [HttpGet("{taskId}")]
        [Description("Get task by Id with tags")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public TaskInfoOutputModel GetTaskWithTags(int taskId)
        {
            var taskDto = _taskService.GetTaskById(taskId);
            return _mapper.Map<TaskInfoOutputModel>(taskDto);
        }

        //  api/Task/courses
        [Authorize(Roles = "Teacher")]
        [HttpGet("{taskId}/with-courses")]
        [Description("Get task by Id with tags and courses")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TaskInfoWithCoursesOutputModel))]
        public TaskInfoWithCoursesOutputModel GetTaskWithTagsAndCourses(int taskId)
        {
            var taskDto = _taskService.GetTaskWithCoursesById(taskId);
            return _mapper.Map<TaskInfoWithCoursesOutputModel>(taskDto);
        }

        //  api/Task/answers
        [HttpGet("{taskId}/with-answers")]
        [Description("Get task by Id with tags and answers")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TaskInfoWithAnswersOutputModel))]
        public TaskInfoWithAnswersOutputModel GetTaskWithTagsAndAnswers(int taskId)
        {
            var taskDto = _taskService.GetTaskWithAnswersById(taskId);
            return _mapper.Map<TaskInfoWithAnswersOutputModel>(taskDto);
        }

        //  api/Task/coursesandanswers
        [HttpGet("{taskId}/full-info")]
        [Description("Get task by Id with tags, courses and answers")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TaskInfoWithCoursesAndAnswersOutputModel))]
        public TaskInfoWithCoursesAndAnswersOutputModel GetTaskWithTagsCoursesAndAnswers(int taskId)
        {
            var taskDto = _taskService.GetTaskWithCoursesAndAnswersById(taskId);
            return _mapper.Map<TaskInfoWithCoursesAndAnswersOutputModel>(taskDto);
        }

        //  api/Task
        [HttpGet]
        [Description("Get all tasks with tags")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TaskInfoOutputModel))]
        public List<TaskInfoOutputModel> GetAllTasksWithTags()
        {
            var taskDtos = _taskService.GetTasks();
            return _mapper.Map<List<TaskInfoOutputModel>>(taskDtos);
        }

        // api/task
        [HttpPost]
        [Description("Add new task")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public TaskInfoOutputModel AddTask([FromBody] TaskInputModel model)
        {
            var taskDto = _mapper.Map<TaskDto>(model);
            return _mapper.Map<TaskInfoOutputModel>(_taskService.GetTaskById(_taskService.AddTask(taskDto)));
        }


        // api/task/{taskId}
        [HttpPut("{taskId}")]
        [Description("Update task")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public TaskInfoOutputModel UpdateTask(int taskId, [FromBody] TaskInputModel model)
        {
            TaskDto taskDto = _mapper.Map<TaskDto>(model);
            taskDto.Id = taskId;
            return _mapper.Map<TaskInfoOutputModel>(_taskService.UpdateTask(taskDto));
        }

        // api/task/{taskId}
        [HttpDelete("{taskId}")]
        [Description("Delete task with selected Id")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public void DeleteTask(int taskId)
        {
            _taskService.DeleteTask(taskId);
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
            taskAnswerDto.Task = new TaskDto { Id = taskId };
            taskAnswerDto.User = new UserDto { Id = studentId };

            _studentAnswerOnTaskService.AddStudentAnswerOnTask(taskAnswerDto);

        }

        // api/task/{taskId}/student
        [HttpGet("student")]
        public List<StudentAnswerOnTaskDto> GetAllStudentAnswersOnTasks()
        {
            var studentStatusDto = _studentAnswerOnTaskService.GetAllStudentAnswersOnTasks();
            return studentStatusDto;
        }

        // api/task/{taskId}/student/{studentId}
        [HttpGet("{taskId}/student/{studentId}")]
        public StudentAnswerOnTaskDto GetStudentAnswerOnTaskByTaskIdAndStudentId(int taskId, int studentId)
        {
            StudentAnswerOnTaskDto dto = new StudentAnswerOnTaskDto();
            dto.Task = new TaskDto { Id = taskId };
            dto.User = new UserDto { Id = studentId };
            dto.Comments = new List<CommentDto>();

            var studentStatusDto = _studentAnswerOnTaskService.GetStudentAnswerOnTaskByTaskIdAndStudentId(dto);
            return studentStatusDto;
        }


        // api/task/{taskId}/student/{studentId}
        [HttpPut("{taskId}/student/{studentId}")]
        public int UpdateStudentAnswerOnTask(int taskId, int studentId, [FromBody] StudentAnswerOnTaskInputModel inputModel)
        {
            var taskAnswerDto = _mapper.Map<StudentAnswerOnTaskDto>(inputModel);
            taskAnswerDto.Task.Id = taskId;
            taskAnswerDto.User.Id = studentId;

            _studentAnswerOnTaskService.UpdateStudentAnswerOnTask(taskAnswerDto);

            return taskId;
        }

        // api/task/{taskId}/student/{studentId}
        [HttpDelete("{taskId}/student/{studentId}")]
        public string DeleteStudentAnswerOnTask(int taskId, int studentId)
        {
            StudentAnswerOnTaskDto dto = new StudentAnswerOnTaskDto();
            dto.Task = new TaskDto { Id = taskId };
            dto.User = new UserDto { Id = studentId };
            dto.Comments = new List<CommentDto>();

            _studentAnswerOnTaskService.DeleteStudentAnswerOnTask(dto);

            return $"Deleted answer for task {taskId} id.";
        }

        // api/task/{taskId}/student/{studentId}/change-status/{statusId}
        [HttpPut("{taskId}/student/{studentId}/change-status/{statusId}")]
        public int UpdateStatusOfStudentAnswer(int taskId, int studentId, int statusId)
        {
            StudentAnswerOnTaskDto dto = new StudentAnswerOnTaskDto();
            dto.Task = new TaskDto { Id = taskId };
            dto.User = new UserDto { Id = studentId };
            dto.TaskStatus = (DAL.Enums.TaskStatus)statusId;
            dto.Comments = new List<CommentDto>();

            _studentAnswerOnTaskService.ChangeStatusOfStudentAnswerOnTask(dto);

            return statusId;
        }

        // api/task/answer/{taskStudentId}/comment}
        [HttpPost("answer/{taskStudentId}/comment")]
        public int AddCommentOnStudentAnswer(int taskstudentId, [FromBody] CommentAddInputModel inputModel)
        {
            var commentDto = _mapper.Map<CommentDto>(inputModel);
            int commentId = _commentRepository.AddComment(commentDto);
            _studentAnswerOnTaskService.AddCommentOnStudentAnswer(taskstudentId, commentId);

            return taskstudentId;
        }
    }
}