using DevEdu.DAL.Models;
using System.Collections.Generic;

namespace DevEdu.DAL.Repositories
{
    public interface IStudentAnswerOnTaskRepository
    {
        void AddStudentAnswerOnTask(StudentAnswerOnTaskDto studentResponse);
        void DeleteStudentAnswerOnTask(int taskId, int studentId);
        List<StudentAnswerOnTaskDto> GetAllStudentAnswerOnTask();
        List<StudentAnswerOnTaskDto> GetStudentAnswerByTaskIdAndStudentIdOnTask(StudentAnswerOnTaskDto studentResponse);
        void UpdateStatusAnswerOnTask(int taskId, int studentId, int statusId);
        void UpdateStudentAnswerOnTask(StudentAnswerOnTaskDto studentResponse);
        void AddCommentOnStudentAnswer(int taskId, int studentId, int commentId);

    }
}