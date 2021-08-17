using DevEdu.Business.IdentityInfo;
using DevEdu.DAL.Models;
using System.Collections.Generic;

namespace DevEdu.Business.Services
{
    public interface IStudentHomeworkService
    {
        StudentHomeworkDto AddStudentHomework(int homeworkId, StudentHomeworkDto taskAnswerDto, UserIdentityInfo userInfo);
        void DeleteStudentHomework(int id, UserIdentityInfo userInfo);
        StudentHomeworkDto UpdateStudentHomework(int id, StudentHomeworkDto updatedDto, UserIdentityInfo userInfo);
        int UpdateStatusOfStudentHomework(int id, int statusId, UserIdentityInfo userInfo);
        StudentHomeworkDto GetStudentHomeworkById(int id, UserIdentityInfo userInfo);
        List<StudentHomeworkDto> GetAllStudentHomeworkOnTask(int taskId);
        List<StudentHomeworkDto> GetAllStudentHomeworkByStudentId(int userId, UserIdentityInfo userInfo);
    }
}