using DevEdu.DAL.Models;
using System.Collections.Generic;

namespace DevEdu.DAL.Repositories
{
    public interface IStudentAnswerOnTaskRepository
    {
        int AddStudentAnswerOnTask(StudentAnswerOnTaskDto taskAnswerDto);
        void DeleteStudentAnswerOnTask(StudentAnswerOnTaskDto dto);
        List<StudentAnswerOnTaskDto> GetAllStudentAnswersOnTask(int taskId);
        StudentAnswerOnTaskDto GetStudentAnswerOnTaskByTaskIdAndStudentId(StudentAnswerOnTaskDto dto);
        void ChangeStatusOfStudentAnswerOnTask(StudentAnswerOnTaskDto dto);
        void UpdateStudentAnswerOnTask(StudentAnswerOnTaskDto dto);
        List<StudentAnswerOnTaskForTaskDto> GetStudentAnswersToTaskByTaskId(int id);
        List<StudentAnswerOnTaskDto> GetAllAnswersByStudentId(int userId);
        StudentAnswerOnTaskDto GetStudentAnswerOnTaskById(int id);
    }
}