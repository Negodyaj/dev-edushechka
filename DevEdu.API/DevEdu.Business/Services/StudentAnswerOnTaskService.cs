using System.Collections.Generic;
using DevEdu.DAL.Enums;
using DevEdu.DAL.Models;
using DevEdu.DAL.Repositories;

namespace DevEdu.Business.Services
{
    public class StudentAnswerOnTaskService: IStudentAnswerOnTaskService
    {
        private readonly IStudentAnswerOnTaskRepository _studentAnswerOnTaskRepository;

        public StudentAnswerOnTaskService(IStudentAnswerOnTaskRepository studentAnswerOnTaskRepository)
        {
            _studentAnswerOnTaskRepository = studentAnswerOnTaskRepository;
        }

        public void AddStudentAnswerOnTask(int taskId, int studentId, StudentAnswerOnTaskDto taskAnswerDto)
        {
            taskAnswerDto.Task = new TaskDto { Id = taskId };
            taskAnswerDto.User = new UserDto { Id = studentId };

            _studentAnswerOnTaskRepository.AddStudentAnswerOnTask(taskAnswerDto);
        }

        public void DeleteStudentAnswerOnTask(int taskId, int studentId)
        {
            StudentAnswerOnTaskDto dto = new StudentAnswerOnTaskDto();
            dto.Task = new TaskDto { Id = taskId };
            dto.User = new UserDto { Id = studentId };
            dto.Comments = new List<CommentDto>();

            _studentAnswerOnTaskRepository.DeleteStudentAnswerOnTask(dto);
        }

        public List<StudentAnswerOnTaskDto> GetAllStudentAnswersOnTasks() => _studentAnswerOnTaskRepository.GetAllStudentAnswersOnTasks();

        public List<StudentAnswerOnTaskDto> GetAllStudentAnswersOnTask(int taskId)
        {
            StudentAnswerOnTaskDto dto = new StudentAnswerOnTaskDto();
            dto.Task = new TaskDto { Id = taskId };
            dto.User = new UserDto();
            dto.Comments = new List<CommentDto>();

            return _studentAnswerOnTaskRepository.GetAllStudentAnswersOnTask(taskId);
        }

        public StudentAnswerOnTaskDto GetStudentAnswerOnTaskByTaskIdAndStudentId(int taskId, int studentId)
        {
            StudentAnswerOnTaskDto dto = new StudentAnswerOnTaskDto();
            dto.Task = new TaskDto { Id = taskId };
            dto.User = new UserDto { Id = studentId };
            dto.Comments = new List<CommentDto>();

            return _studentAnswerOnTaskRepository.GetStudentAnswerOnTaskByTaskIdAndStudentId(dto);
        }

        public void ChangeStatusOfStudentAnswerOnTask(int taskId, int studentId, int statusId) 
        {
            StudentAnswerOnTaskDto dto = new StudentAnswerOnTaskDto();
            dto.Task = new TaskDto { Id = taskId };
            dto.User = new UserDto { Id = studentId };
            dto.TaskStatus = (TaskStatus)statusId;

            if (dto.TaskStatus == TaskStatus.Accepted)
                dto.CompletedDate = System.DateTime.Now;

            _studentAnswerOnTaskRepository.ChangeStatusOfStudentAnswerOnTask(dto);
        }

        public void UpdateStudentAnswerOnTask(int taskId, int studentId, StudentAnswerOnTaskDto taskAnswerDto)
        {
            taskAnswerDto.Task = new TaskDto { Id = taskId };
            taskAnswerDto.User = new UserDto { Id = studentId };

            _studentAnswerOnTaskRepository.UpdateStudentAnswerOnTask(taskAnswerDto);
        }

        public void AddCommentOnStudentAnswer(int taskstudentId, int commentId) => _studentAnswerOnTaskRepository.AddCommentOnStudentAnswer(taskstudentId, commentId);


    }
}
