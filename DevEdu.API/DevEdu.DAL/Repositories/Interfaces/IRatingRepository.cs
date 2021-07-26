using DevEdu.DAL.Models;
using System.Collections.Generic;

namespace DevEdu.DAL.Repositories
{
    public interface IRatingRepository
    {
        int AddStudentRating(StudentRatingDto studentRatingDto);
        void DeleteStudentRating(int id);
        List<StudentRatingDto> SelectAllStudentRatings();
        StudentRatingDto SelectStudentRatingById(int id);
        List<StudentRatingDto> SelectStudentRatingByUserId(int userId);
        public List<StudentRatingDto> SelectStudentRatingByGroupId(int groupId);
        void UpdateStudentRating(StudentRatingDto studentRatingDto);
    }
}