using DevEdu.DAL.Models;
using System.Collections.Generic;

namespace DevEdu.Business.Services
{
    public interface IStudentAnswerOnTaskService
    {
        int AddStudentAnswerOnTask(int taskId, int studentId, StudentAnswerOnTaskDto taskAnswerDto);
        void DeleteStudentAnswerOnTask(int taskId, int studentId);
        List<StudentAnswerOnTaskDto> GetAllStudentAnswersOnTasks();
        List<StudentAnswerOnTaskDto> GetAllStudentAnswersOnTask(int taskId);
        StudentAnswerOnTaskDto GetStudentAnswerOnTaskByTaskIdAndStudentId(int taskId, int studentId);
        void ChangeStatusOfStudentAnswerOnTask(int taskId, int studentId, int statusId);
        void UpdateStudentAnswerOnTask(int taskId, int studentId, StudentAnswerOnTaskDto taskAnswerDto);
        void AddCommentOnStudentAnswer(int taskstudentId, int commentId);
        List<StudentAnswerOnTaskDto> GetAnswersForStudentInGroup(int userId);
    }
}
