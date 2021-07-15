using DevEdu.DAL.Models;
using System.Collections.Generic;

namespace DevEdu.Business.Services
{
    public interface IRaitingService
    {
        int AddStudentRaiting(StudentRaitingDto studentRaitingDto);
        void DeleteStudentRaiting(int id);
        List<StudentRaitingDto> GetAllStudentRaitings();
        StudentRaitingDto GetStudentRaitingById(int id);
        List<StudentRaitingDto> GetStudentRaitingByUserId(int userId);
        void UpdateStudentRaiting(StudentRaitingDto studentRaitingDto);
    }
}