using DevEdu.DAL.Models;
using System.Collections.Generic;

namespace DevEdu.Business.Services
{
    public interface IStudentAnswerOnTaskService
    {
        void AddStudentAnswerOnTask(StudentAnswerOnTaskDto studentResponse);
        void DeleteStudentAnswerOnTask(int taskId, int studentId);
        List<StudentAnswerOnTaskDto> GetAllStudentAnswersOnTask();
        StudentAnswerOnTaskDto GetStudentAnswerByTaskIdAndStudentIdOnTask(int taskId, int studentId);
        void UpdateStatusAnswerOnTask(int taskId, int studentId, int statusId);
        void UpdateStudentAnswerOnTask(StudentAnswerOnTaskDto studentResponse);
        void AddCommentOnStudentAnswer(int taskstudentId, int commentId);
    }
}
