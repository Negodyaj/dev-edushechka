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
        public StudentAnswerOnTaskDto GetStudentAnswerByTaskIdAndStudentIdOnTask(int taskId, int studentId) => GetStudentAnswerByTaskIdAndStudentIdOnTask(taskId, studentId);
        public void UpdateStatusAnswerOnTask(int taskId, int studentId, int statusId) => _studentAnswerOnTaskService.UpdateStatusAnswerOnTask(taskId, studentId, statusId);
        public void UpdateStudentAnswerOnTask(StudentAnswerOnTaskDto studentResponse) => _studentAnswerOnTaskService.UpdateStudentAnswerOnTask(studentResponse);
        public void AddCommentOnStudentAnswer(int taskstudentId, int commentId) => _studentAnswerOnTaskService.AddCommentOnStudentAnswer(taskstudentId, commentId);
    }
}
