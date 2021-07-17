using System.Collections.Generic;
using DevEdu.DAL.Models;

namespace DevEdu.Business.Services
{
    public class StudentAnswerOnTaskService: IStudentAnswerOnTaskService
    {
        private readonly IStudentAnswerOnTaskService _studentAnswerOnTaskService;

        public StudentAnswerOnTaskService(IStudentAnswerOnTaskService studentAnswerOnTaskService)
        {
            _studentAnswerOnTaskService = studentAnswerOnTaskService;
        }

        public void AddStudentAnswerOnTask(StudentAnswerOnTaskDto studentResponse) => _studentAnswerOnTaskService.AddStudentAnswerOnTask(studentResponse);
        public void DeleteStudentAnswerOnTask(int taskId, int studentId) => _studentAnswerOnTaskService.DeleteStudentAnswerOnTask(taskId, studentId);
        public List<StudentAnswerOnTaskDto> GetAllStudentAnswersOnTask() => _studentAnswerOnTaskService.GetAllStudentAnswersOnTask();
        public StudentAnswerOnTaskDto GetStudentAnswerOnTaskByTaskIdAndStudentId(int taskId, int studentId) => GetStudentAnswerOnTaskByTaskIdAndStudentId(taskId, studentId);
        public void ChangeStatusOfStudentAnswerOnTask(int taskId, int studentId, int statusId) => _studentAnswerOnTaskService.ChangeStatusOfStudentAnswerOnTask(taskId, studentId, statusId);
        public void UpdateStudentAnswerOnTask(StudentAnswerOnTaskDto dto) => _studentAnswerOnTaskService.UpdateStudentAnswerOnTask(dto);
        public void AddCommentOnStudentAnswer(int taskstudentId, int commentId) => _studentAnswerOnTaskService.AddCommentOnStudentAnswer(taskstudentId, commentId);
    }
}
