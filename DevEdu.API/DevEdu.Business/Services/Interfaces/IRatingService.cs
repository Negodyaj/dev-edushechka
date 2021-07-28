using DevEdu.DAL.Models;
using System.Collections.Generic;

namespace DevEdu.Business.Services
{
    public interface IRatingService
    {
        int AddStudentRating(StudentRatingDto studentRatingDto, int authorUserId);
        void DeleteStudentRating(int id, int authorUserId);
        List<StudentRatingDto> GetAllStudentRatings();
        StudentRatingDto GetStudentRatingById(int id);
        List<StudentRatingDto> GetStudentRatingByUserId(int userId);
        public List<StudentRatingDto> GetStudentRatingByGroupId(int groupId);
        StudentRatingDto UpdateStudentRating(int id, int value, int periodNumber, int authorUserId);
    }
}