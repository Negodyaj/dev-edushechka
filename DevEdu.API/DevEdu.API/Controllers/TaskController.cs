using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DevEdu.API.Models.InputModels;
using System.Collections.Generic;
using DevEdu.DAL.Repositories;
using DevEdu.DAL.Models;
using AutoMapper;

namespace DevEdu.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TaskController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IStudentAnswerOnTaskRepository _studentAnswerOnTaskRepository;


        public TaskController(IMapper mapper,  IStudentAnswerOnTaskRepository studentAnswerOnTaskRepository)
        {
            _mapper = mapper;

            _studentAnswerOnTaskRepository = studentAnswerOnTaskRepository;
        }

        //  api/Task/1
        [HttpGet("{id}")]
        public string GetTask(int taskId)
        {
            return $"Get task №{taskId}";
        }

        //  api/Task
        [HttpGet]
        public string GetAllTasks()
        {
            return "All Tasks";
        }

        // api/task
        [HttpPost]
        public int AddTask([FromBody] TaskInputModel model)
        {

            return 1;
        }

        // api/task/{taskId}
        [HttpDelete("{taskId}")]
        public string DeleteTask(int taskId)
        {
            return $"deleted task with {taskId} Id";
        }

        // api/task/{taskId}
        [HttpPut("{taskId}")]
        public string UpdateTask(int taskId, [FromBody] TaskInputModel model)
        {

            return $"update task with {taskId} Id";
        }

        // api/task/{taskId}/tag/{tagId}
        [HttpPost("{taskId}/tag/{tagId}")]
        public int AddTagToTask(int taskId, int tagId)
        {
            return 1;
        }

        // api/task/{taskId}/tag/{tagId}
        [HttpDelete("{taskId}/tag/{tagId}")]
        public string DeleteTagFromTask(int taskId, int tagId)
        {
            return $"deleted tag task with {taskId} taskId";
        }

        // api/task/{taskId}/student/{studentId}
        [HttpPost("{taskId}/student/{studentId}")]
        public string AddStudentAnswerOnTask(int taskId, int studentId, [FromBody] StudentAnswerOnTaskInputModel studentResponse)
        {
            var taskAnswerDto = _mapper.Map<StudentAnswerOnTaskDto>(studentResponse);
            taskAnswerDto.TaskId = taskId;
            taskAnswerDto.StudentId = studentId;

            return _studentAnswerOnTaskRepository.AddStudentAnswerOnTaskDto(taskAnswerDto);

        }

        // api/task/{taskId}/student/{studentId}
        [HttpPut("{taskId}/student/{studentId}")]
        public int UpdateStudentAnswerOnTask(int taskId, int studentId, [FromBody] StudentAnswerOnTaskInputModel studentResponse)
        {
            var taskAnswerDto = _mapper.Map<StudentAnswerOnTaskDto>(studentResponse);
            taskAnswerDto.TaskId = taskId;
            taskAnswerDto.StudentId = studentId;

            _studentAnswerOnTaskRepository.UpdateStudentAnswerOnTaskDto(taskAnswerDto);

            return taskId;
        }

        // api/task/{taskId}/student/{studentId}
        [HttpDelete("{taskId}/student/{studentId}")]
        public string DeleteStudentAnswerOnTask(int taskId, int studentId, [FromBody] StudentAnswerOnTaskDto studentResponse)
        {
            var taskAnswerDto = _mapper.Map<StudentAnswerOnTaskDto>(studentResponse);
            taskAnswerDto.TaskId = taskId;
            taskAnswerDto.StudentId = studentId;

            _studentAnswerOnTaskRepository.DeleteStudentAnswerOnTaskDto(studentResponse);

            return $"Deleted answer for task {taskId} id.";
        }

        // api/task/{taskId}/student/{studentId}/change-status/{statusId}
        [HttpPut("{taskId}/student/{studentId}/change-status/{statusId}")]
        public int UpdateStatusOfStudentAnswer(int taskId, int studentId, int statusId, [FromBody] StudentAnswerOnTaskDto studentResponse)
        {
            var taskAnswerDto = _mapper.Map<StudentAnswerOnTaskDto>(studentResponse);
            taskAnswerDto.TaskId = taskId;
            taskAnswerDto.StudentId = studentId;
            taskAnswerDto.StatusId = statusId;

            _studentAnswerOnTaskRepository.UpdateStatusAnswerOnTaskDto(studentResponse);

            return statusId;
        }

        // api/task/{taskId}/student/{studentId}/comment}
        [HttpPost("{taskId}/student/{studentId}/comment")]
        public int AddCommentOnStudentAnswer(int taskId, int studentId, [FromBody] CommentAddtInputModel inputModel)
        {
            return taskId;
        }
    }
}
