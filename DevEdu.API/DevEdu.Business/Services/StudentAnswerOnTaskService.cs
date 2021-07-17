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

        public void AddStudentAnswerOnTask(StudentAnswerOnTaskDto dto) => _studentAnswerOnTaskService.AddStudentAnswerOnTask(dto);
        public void DeleteStudentAnswerOnTask(StudentAnswerOnTaskDto dto) => _studentAnswerOnTaskService.DeleteStudentAnswerOnTask(dto);
        public List<StudentAnswerOnTaskDto> GetAllStudentAnswersOnTask() => _studentAnswerOnTaskService.GetAllStudentAnswersOnTask();
        public StudentAnswerOnTaskDto GetStudentAnswerOnTaskByTaskIdAndStudentId(StudentAnswerOnTaskDto dto) => GetStudentAnswerOnTaskByTaskIdAndStudentId(dto);
        public void ChangeStatusOfStudentAnswerOnTask(StudentAnswerOnTaskDto dto, int statusId) => _studentAnswerOnTaskService.ChangeStatusOfStudentAnswerOnTask(dto, statusId);
        public void UpdateStudentAnswerOnTask(StudentAnswerOnTaskDto dto) => _studentAnswerOnTaskService.UpdateStudentAnswerOnTask(dto);
        public void AddCommentOnStudentAnswer(int taskstudentId, int commentId) => _studentAnswerOnTaskService.AddCommentOnStudentAnswer(taskstudentId, commentId);
    }
}
