using DevEdu.Business.IdentityInfo;
using DevEdu.DAL.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DevEdu.Business.Services
{
    public interface IRatingService
    {
        Task<StudentRatingDto> AddStudentRatingAsync(StudentRatingDto studentRatingDto, UserIdentityInfo authorUserInfo);
        Task DeleteStudentRatingAsync(int id, UserIdentityInfo authorUserInfo);
        Task<List<StudentRatingDto>> GetAllStudentRatingsAsync();
        Task<List<StudentRatingDto>> GetStudentRatingByUserIdAsync(int userId);
        Task<List<StudentRatingDto>> GetStudentRatingByGroupIdAsync(int groupId, UserIdentityInfo authorUserInfo);
        Task<StudentRatingDto> UpdateStudentRatingAsync(int id, int value, int periodNumber, UserIdentityInfo authorUserInfo);
    }
}