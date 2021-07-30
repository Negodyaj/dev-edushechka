using DevEdu.DAL.Models;
using System.Collections.Generic;

namespace DevEdu.Business.Services
{
    public interface IStudentAnswerOnTaskService
    {
        int AddStudentAnswerOnTask(int taskId, int studentId, StudentAnswerOnTaskDto taskAnswerDto);
        void DeleteStudentAnswerOnTask(int taskId, int studentId);
        List<StudentAnswerOnTaskDto> GetAllStudentAnswersOnTask(int taskId);
        StudentAnswerOnTaskDto GetStudentAnswerOnTaskByTaskIdAndStudentId(int taskId, int studentId);
        int ChangeStatusOfStudentAnswerOnTask(int taskId, int studentId, int statusId, StudentAnswerOnTaskDto dto);
        StudentAnswerOnTaskDto UpdateStudentAnswerOnTask(int taskId, int studentId, StudentAnswerOnTaskDto taskAnswerDto);
        int AddCommentOnStudentAnswer(int taskStudentId, int commentId);
        List<StudentAnswerOnTaskDto> GetAllAnswersByStudentId(int userId);
    }
}