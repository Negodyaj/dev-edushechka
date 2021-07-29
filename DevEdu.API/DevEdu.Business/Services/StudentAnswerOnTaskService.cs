using System;
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

        public int AddStudentAnswerOnTask(int taskId, int studentId, StudentAnswerOnTaskDto taskAnswerDto)
        {
            taskAnswerDto.Task = new TaskDto { Id = taskId };
            taskAnswerDto.User = new UserDto { Id = studentId };

            var studentAnswerId = _studentAnswerOnTaskRepository.AddStudentAnswerOnTask(taskAnswerDto);

            return studentAnswerId;
        }

        public void DeleteStudentAnswerOnTask(int taskId, int studentId)
        {
            StudentAnswerOnTaskDto dto = new StudentAnswerOnTaskDto();
            dto.Task = new TaskDto { Id = taskId };
            dto.User = new UserDto { Id = studentId };
            dto.Comments = new List<CommentDto>();

            _studentAnswerOnTaskRepository.DeleteStudentAnswerOnTask(dto);
        }

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
            StudentAnswerOnTaskDto studentAnswerDto =
                new StudentAnswerOnTaskDto
                {
                    Task = new TaskDto { Id = taskId },
                    User = new UserDto { Id = studentId },
                    Comments = new List<CommentDto>()
                };



            var answerDto = _studentAnswerOnTaskRepository.GetStudentAnswerOnTaskByTaskIdAndStudentId(studentAnswerDto);
            return answerDto;
        }

        public int ChangeStatusOfStudentAnswerOnTask(int taskId, int studentId, int statusId) 
        {
            StudentAnswerOnTaskDto dto = new StudentAnswerOnTaskDto();
            dto.Task = new TaskDto { Id = taskId };
            dto.User = new UserDto { Id = studentId };
            dto.TaskStatus = (TaskStatus)statusId;

            if (dto.TaskStatus == TaskStatus.Accepted)
                dto.CompletedDate = System.DateTime.Now;

            _studentAnswerOnTaskRepository.ChangeStatusOfStudentAnswerOnTask(dto);

            return (int)dto.TaskStatus;
        }

        public StudentAnswerOnTaskDto UpdateStudentAnswerOnTask(int taskId, int studentId, StudentAnswerOnTaskDto taskAnswerDto)
        {
            taskAnswerDto.Task = new TaskDto { Id = taskId };
            taskAnswerDto.User = new UserDto { Id = studentId };

            _studentAnswerOnTaskRepository.UpdateStudentAnswerOnTask(taskAnswerDto);

            return _studentAnswerOnTaskRepository.GetStudentAnswerOnTaskByTaskIdAndStudentId(taskAnswerDto);
        }

        public void AddCommentOnStudentAnswer(int taskStudentId, int commentId) => _studentAnswerOnTaskRepository.AddCommentOnStudentAnswer(taskStudentId, commentId);

        public List<StudentAnswerOnTaskDto> GetAllAnswersByStudentId(int userId)
        {
            var dto = _studentAnswerOnTaskRepository.GetAllAnswersByStudentId(userId);
            return dto;
        }
    }
}
