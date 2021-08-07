using DevEdu.Business.IdentityInfo;
using DevEdu.DAL.Models;
using System.Collections.Generic;

namespace DevEdu.Business.Services
{
    public interface IRatingService
    {
        StudentRatingDto AddStudentRating(StudentRatingDto studentRatingDto, UserIdentityInfo authorUserInfo);
        void DeleteStudentRating(int id, UserIdentityInfo authorUserInfo);
        List<StudentRatingDto> GetAllStudentRatings();
        List<StudentRatingDto> GetStudentRatingByUserId(int userId);
        public List<StudentRatingDto> GetStudentRatingByGroupId(int groupId, UserIdentityInfo authorUserInfo);
        StudentRatingDto UpdateStudentRating(int id, int value, int periodNumber, UserIdentityInfo authorUserInfo);
    }
}