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

        public void AddStudentAnswerOnTask(StudentAnswerOnTaskDto dto) => _studentAnswerOnTaskRepository.AddStudentAnswerOnTask(dto);
        public void DeleteStudentAnswerOnTask(StudentAnswerOnTaskDto dto) => _studentAnswerOnTaskRepository.DeleteStudentAnswerOnTask(dto);
        public List<StudentAnswerOnTaskDto> GetAllStudentAnswersOnTasks() => _studentAnswerOnTaskRepository.GetAllStudentAnswersOnTask();
        public StudentAnswerOnTaskDto GetStudentAnswerOnTaskByTaskIdAndStudentId(StudentAnswerOnTaskDto dto) => _studentAnswerOnTaskRepository.GetStudentAnswerOnTaskByTaskIdAndStudentId(dto);
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
        public void UpdateStudentAnswerOnTask(StudentAnswerOnTaskDto dto) => _studentAnswerOnTaskRepository.UpdateStudentAnswerOnTask(dto);
        public void AddCommentOnStudentAnswer(int taskstudentId, int commentId) => _studentAnswerOnTaskRepository.AddCommentOnStudentAnswer(taskstudentId, commentId);
    }
}
