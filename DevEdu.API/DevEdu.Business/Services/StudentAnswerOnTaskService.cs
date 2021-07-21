using System.Collections.Generic;
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
        public List<StudentAnswerOnTaskDto> GetAllStudentAnswersOnTask() => _studentAnswerOnTaskRepository.GetAllStudentAnswersOnTask();
        public StudentAnswerOnTaskDto GetStudentAnswerOnTaskByTaskIdAndStudentId(StudentAnswerOnTaskDto dto) => _studentAnswerOnTaskRepository.GetStudentAnswerOnTaskByTaskIdAndStudentId(dto);
        public void ChangeStatusOfStudentAnswerOnTask(StudentAnswerOnTaskDto dto) => _studentAnswerOnTaskRepository.ChangeStatusOfStudentAnswerOnTask(dto);
        public void UpdateStudentAnswerOnTask(StudentAnswerOnTaskDto dto) => _studentAnswerOnTaskRepository.UpdateStudentAnswerOnTask(dto);
        public void AddCommentOnStudentAnswer(int taskstudentId, int commentId) => _studentAnswerOnTaskRepository.AddCommentOnStudentAnswer(taskstudentId, commentId);
    }
}
