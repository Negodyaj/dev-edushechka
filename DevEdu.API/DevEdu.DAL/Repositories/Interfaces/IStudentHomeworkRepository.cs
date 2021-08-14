using DevEdu.DAL.Models;
using System;
using System.Collections.Generic;

namespace DevEdu.DAL.Repositories
{
    public interface IStudentHomeworkRepository
    {
        int AddStudentAnswerOnHomework(StudentHomeworkDto taskAnswerDto);
        void DeleteStudentHomework(int id);
        List<StudentHomeworkDto> GetAllStudentAnswersOnTask(int taskId);
        int ChangeStatusOfStudentAnswerOnTask(int id, int statusId, DateTime completedDate);
        void UpdateStudentAnswerOnTask(StudentHomeworkDto dto);
        List<StudentHomeworkDto> GetAllAnswersByStudentId(int userId);
        StudentHomeworkDto GetStudentHomeworkById(int id);
    }
}