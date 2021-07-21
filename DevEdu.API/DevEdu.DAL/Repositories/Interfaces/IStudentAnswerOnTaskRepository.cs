using DevEdu.DAL.Models;
using System.Collections.Generic;

namespace DevEdu.DAL.Repositories
{
    public interface IStudentAnswerOnTaskRepository
    {
        void AddStudentAnswerOnTask(StudentAnswerOnTaskDto taskAnswerDto);
        void DeleteStudentAnswerOnTask(StudentAnswerOnTaskDto dto);
        List<StudentAnswerOnTaskDto> GetAllStudentAnswersOnTasks();
        List<StudentAnswerOnTaskDto> GetAllStudentAnswersOnTask(int taskId);
        StudentAnswerOnTaskDto GetStudentAnswerOnTaskByTaskIdAndStudentId(StudentAnswerOnTaskDto dto);
        void ChangeStatusOfStudentAnswerOnTask(StudentAnswerOnTaskDto dto);
        void UpdateStudentAnswerOnTask(StudentAnswerOnTaskDto dto);
        void AddCommentOnStudentAnswer(int taskstudentId, int commentId);
        List<StudentAnswerOnTaskForTaskDto> GetStudentAnswersToTaskByTaskId(int id);
    }
}