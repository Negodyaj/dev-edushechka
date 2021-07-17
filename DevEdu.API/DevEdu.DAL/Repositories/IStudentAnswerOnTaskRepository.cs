using DevEdu.DAL.Models;
using System.Collections.Generic;

namespace DevEdu.DAL.Repositories
{
    public interface IStudentAnswerOnTaskRepository
    {
        void AddStudentAnswerOnTask(StudentAnswerOnTaskDto dto);
        void DeleteStudentAnswerOnTask(int taskId, int studentId);
        List<StudentAnswerOnTaskDto> GetAllStudentAnswersOnTask();
        StudentAnswerOnTaskDto GetStudentAnswerOnTaskByTaskIdAndStudentId(int taskId, int studentId);
        void ChangeStatusOfStudentAnswerOnTask(StudentAnswerOnTaskDto dto, int statusId);
        void UpdateStudentAnswerOnTask(StudentAnswerOnTaskDto dto);
        void AddCommentOnStudentAnswer(int taskstudentId, int commentId);

    }
}