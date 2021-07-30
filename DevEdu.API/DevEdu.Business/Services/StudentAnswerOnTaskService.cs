using DevEdu.DAL.Enums;
using DevEdu.DAL.Models;
using DevEdu.DAL.Repositories;
using System.Collections.Generic;

namespace DevEdu.Business.Services
{
    public class StudentAnswerOnTaskService : IStudentAnswerOnTaskService
    {
        private readonly IStudentAnswerOnTaskRepository _studentAnswerOnTaskRepository;
        private readonly IGroupRepository _groupRepository;

        public StudentAnswerOnTaskService(
            IStudentAnswerOnTaskRepository studentAnswerOnTaskRepository,
            IGroupRepository groupRepository)
        {
            _studentAnswerOnTaskRepository = studentAnswerOnTaskRepository;
            _groupRepository = groupRepository;
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

        public void AddCommentOnStudentAnswer(int taskStudentId, int commentId) => _studentAnswerOnTaskRepository.AddCommentOnStudentAnswer(taskStudentId, commentId);

        public List<StudentAnswerOnTaskDto> GetAllAnswersByStudentId(int userId)
        {
            var dto = _studentAnswerOnTaskRepository.GetAllAnswersByStudentId(userId);
            return dto;
        }
    }
}