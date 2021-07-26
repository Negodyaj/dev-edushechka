using DevEdu.DAL.Models;
using DevEdu.DAL.Repositories;
using System.Collections.Generic;

namespace DevEdu.Business.Services
{
    public class RatingService : IRatingService
    {
        private readonly IRatingRepository _repository;

        public RatingService(IRatingRepository repository)
        {
            _repository = repository;
        }

        public int AddStudentRating(StudentRatingDto studentRatingDto) => _repository.AddStudentRating(studentRatingDto);

        public void DeleteStudentRating(int id) => _repository.DeleteStudentRating(id);

        public List<StudentRatingDto> GetAllStudentRatings() => _repository.SelectAllStudentRatings();

        public StudentRatingDto GetStudentRatingById(int id) => _repository.SelectStudentRatingById(id);

        public List<StudentRatingDto> GetStudentRatingByUserId(int userId) => _repository.SelectStudentRatingByUserId(userId);

        public List<StudentRatingDto> GetStudentRatingByGroupId(int groupId) => _repository.SelectStudentRatingByGroupId(groupId);

        public StudentRatingDto UpdateStudentRating(int id, int value, int periodNumber)
        {
            var dto = new StudentRatingDto
            {
                Id = id,
                Rating = value,
                ReportingPeriodNumber = periodNumber
            };
            _repository.UpdateStudentRating(dto);
            return GetStudentRatingById(id);
        }
    }
}
