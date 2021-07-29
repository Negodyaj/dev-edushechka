using DevEdu.DAL.Enums;
using DevEdu.DAL.Models;
using System.Collections.Generic;

namespace DevEdu.Business.Services
{
    public interface IRatingService
    {
        StudentRatingDto AddStudentRating(StudentRatingDto studentRatingDto, int authorUserId);
        void DeleteStudentRating(int id, int authorUserId);
        List<StudentRatingDto> GetAllStudentRatings();
        List<StudentRatingDto> GetStudentRatingByUserId(int userId);
        public List<StudentRatingDto> GetStudentRatingByGroupId(int groupId, int authorUserId, List<Role> authRoles);
        StudentRatingDto UpdateStudentRating(int id, int value, int periodNumber, int authorUserId);
    }
}