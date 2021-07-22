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
        public List<StudentRaitingDto> GetStudentRaitingByGroupId(int groupId);
        StudentRaitingDto UpdateStudentRaiting(int id, int value, int periodNumber);
    }
}