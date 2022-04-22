using DevEdu.Business.IdentityInfo;
using DevEdu.DAL.Enums;
using DevEdu.DAL.Models;
using System.Threading.Tasks;

namespace DevEdu.Business.ValidationHelpers
{
    public interface IStudentHomeworkValidationHelper
    {
        Task CheckUserInStudentHomeworkAccessAsync(int studentId, int userId);
        Task CheckUserBelongsToHomeworkAsync(int groupId, int userId);
        Task<StudentHomeworkDto> GetStudentHomeworkByIdAndThrowIfNotFoundAsync(int id);
        Task CheckUserComplianceToStudentHomeworkAsync(int studentId, int userId);
        void CheckUserCanChangeStatus(UserIdentityInfo userIdentityInfo, StudentHomeworkDto studentHomeworkDto,
                                            StudentHomeworkStatus newStatus);
    }
}