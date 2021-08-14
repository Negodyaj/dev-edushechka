using DevEdu.DAL.Models;
using System;
using System.Collections.Generic;

namespace DevEdu.DAL.Repositories
{
    public interface IStudentHomeworkRepository
    {
        int AddStudentHomework(StudentHomeworkDto taskAnswerDto);
        void DeleteStudentHomework(int id);
        List<StudentHomeworkDto> GetAllStudentHomeworkByTask(int taskId);
        int ChangeStatusOfStudentAnswerOnTask(int id, int statusId, DateTime completedDate);
        void UpdateStudentHomework(StudentHomeworkDto dto);
        List<StudentHomeworkDto> GetAllStudentHomeworkByStudentId(int userId);
        StudentHomeworkDto GetStudentHomeworkById(int id);
    }
}