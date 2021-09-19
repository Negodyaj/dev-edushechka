using DevEdu.DAL.Models;
using System.Threading.Tasks;

namespace DevEdu.Business.ValidationHelpers
{
    public interface IStudentHomeworkValidationHelper
    {
        Task<StudentHomeworkDto> GetStudentHomeworkByIdAndThrowIfNotFound(int id);
        void CheckUserInStudentHomeworkAccess(int studentId, int userId);
        void CheckUserBelongsToHomework(int groupId, int userId);
        void CheckUserComplianceToStudentHomework(int studentId, int userId);
    }
}