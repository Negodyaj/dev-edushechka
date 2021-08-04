using DevEdu.DAL.Models;
using System;
using System.Collections.Generic;

namespace DevEdu.DAL.Repositories
{
    public interface IStudentAnswerOnTaskRepository
    {
        int AddStudentAnswerOnTask(StudentAnswerOnTaskDto taskAnswerDto);
        void DeleteStudentAnswerOnTask(int taskId, int studentId);
        List<StudentAnswerOnTaskDto> GetAllStudentAnswersOnTask(int taskId);
        StudentAnswerOnTaskDto GetStudentAnswerOnTaskByTaskIdAndStudentId(int taskId, int studentId);
        int ChangeStatusOfStudentAnswerOnTask(int taskId, int studentId, int statusId, DateTime completedDate);
        void UpdateStudentAnswerOnTask(StudentAnswerOnTaskDto dto);
        int AddCommentOnStudentAnswer(int taskstudentId, int commentId);
        List<StudentAnswerOnTaskForTaskDto> GetStudentAnswersToTaskByTaskId(int id);
        List<StudentAnswerOnTaskDto> GetAllAnswersByStudentId(int userId);
    }
}