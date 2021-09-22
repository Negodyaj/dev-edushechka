using DevEdu.DAL.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DevEdu.DAL.Repositories
{
    public interface IRatingRepository
    {
        Task<int> AddStudentRatingAsync(StudentRatingDto studentRatingDto);
        Task DeleteStudentRatingAsync(int id);
        Task<List<StudentRatingDto>> SelectAllStudentRatingsAsync();
        Task<StudentRatingDto> SelectStudentRatingByIdAsync(int id);
        Task<List<StudentRatingDto>> SelectStudentRatingByUserIdAsync(int userId);
        Task<List<StudentRatingDto>> SelectStudentRatingByGroupIdAsync(int groupId);
        Task UpdateStudentRatingAsync(StudentRatingDto studentRatingDto);
    }
}