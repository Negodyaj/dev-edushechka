using DevEdu.Business.IdentityInfo;
using DevEdu.Business.ValidationHelpers;
using DevEdu.DAL.Enums;
using DevEdu.DAL.Models;
using DevEdu.DAL.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DevEdu.Business.Services
{
    public class RatingService : IRatingService
    {
        private readonly IRatingRepository _repository;
        private readonly IRatingValidationHelper _ratingValidationHelper;
        private readonly IGroupValidationHelper _groupValidationHelper;
        private readonly IUserValidationHelper _userValidationHelper;

        public RatingService(IRatingRepository repository, IRatingValidationHelper ratingValidationHelper,
            IGroupValidationHelper groupValidationHelper, IUserValidationHelper userValidationHelper)
        {
            _repository = repository;
            _ratingValidationHelper = ratingValidationHelper;
            _groupValidationHelper = groupValidationHelper;
            _userValidationHelper = userValidationHelper;
        }

        public StudentRatingDto AddStudentRating(StudentRatingDto studentRatingDto, UserIdentityInfo authorUserInfo)
        {
            var groupDto = Task.Run(() => _groupValidationHelper.CheckGroupExistenceAsync(studentRatingDto.Group.Id)).GetAwaiter().GetResult();
            if (!authorUserInfo.IsAdmin())
            {
                _userValidationHelper.CheckAuthorizationUserToGroup(studentRatingDto.Group.Id, authorUserInfo.UserId, Role.Teacher);
            }
            _userValidationHelper.GetUserByIdAndThrowIfNotFound(studentRatingDto.User.Id);
            _userValidationHelper.CheckUserBelongToGroup(studentRatingDto.Group.Id, studentRatingDto.User.Id, Role.Student);
            var id = _repository.AddStudentRating(studentRatingDto);
            return _repository.SelectStudentRatingById(id);
        }

        public void DeleteStudentRating(int id, UserIdentityInfo authorUserInfo)
        {
            var dto = _ratingValidationHelper.CheckRaitingExistenceAndReturnDto(id);
            if (!authorUserInfo.IsAdmin())
            {
                _userValidationHelper.CheckAuthorizationUserToGroup(dto.Group.Id, authorUserInfo.UserId, Role.Teacher);
            }
            _repository.DeleteStudentRating(id);
        }

        public List<StudentRatingDto> GetAllStudentRatings()
        {
            return _repository.SelectAllStudentRatings();
        }

        public List<StudentRatingDto> GetStudentRatingByUserId(int userId)
        {
            _userValidationHelper.GetUserByIdAndThrowIfNotFound(userId);
            return _repository.SelectStudentRatingByUserId(userId);
        }

        public List<StudentRatingDto> GetStudentRatingByGroupId(int groupId, UserIdentityInfo authorUserInfo)
        {
            if (!authorUserInfo.IsAdmin() && !authorUserInfo.IsManager())
            {
                _userValidationHelper.CheckAuthorizationUserToGroup(groupId, authorUserInfo.UserId, Role.Teacher);
            }
            var groupDto = Task.Run(() => _groupValidationHelper.CheckGroupExistenceAsync(groupId)).GetAwaiter().GetResult();
            return _repository.SelectStudentRatingByGroupId(groupId);
        }

        public StudentRatingDto UpdateStudentRating(int id, int value, int periodNumber, UserIdentityInfo authorUserInfo)
        {
            var dto = _ratingValidationHelper.CheckRaitingExistenceAndReturnDto(id);
            if (!authorUserInfo.IsAdmin())
            {
                _userValidationHelper.CheckAuthorizationUserToGroup(dto.Group.Id, authorUserInfo.UserId, Role.Teacher);
            }
            dto = new StudentRatingDto
            {
                Id = id,
                Rating = value,
                ReportingPeriodNumber = periodNumber
            };
            _repository.UpdateStudentRating(dto);
            return _repository.SelectStudentRatingById(id);
        }
    }
}