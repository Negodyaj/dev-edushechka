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

        public async Task<StudentRatingDto> AddStudentRatingAsync(StudentRatingDto studentRatingDto, UserIdentityInfo authorUserInfo)
        {
            await _groupValidationHelper.CheckGroupExistenceAsync(studentRatingDto.Group.Id);
            if (!authorUserInfo.IsAdmin())
            {
                await _userValidationHelper.CheckAuthorizationUserToGroupAsync(studentRatingDto.Group.Id, authorUserInfo.UserId, Role.Teacher);
            }

            await _userValidationHelper.GetUserByIdAndThrowIfNotFoundAsync(studentRatingDto.User.Id);
            await _userValidationHelper.CheckUserBelongToGroupAsync(studentRatingDto.Group.Id, studentRatingDto.User.Id, Role.Student);
            var id = await _repository.AddStudentRatingAsync(studentRatingDto);
            return await _repository.SelectStudentRatingByIdAsync(id);
        }

        public async Task DeleteStudentRatingAsync(int id, UserIdentityInfo authorUserInfo)
        {
            var dto = await _ratingValidationHelper.CheckRaitingExistenceAndReturnDtoAsync(id);
            if (!authorUserInfo.IsAdmin())
            {
                await _userValidationHelper.CheckAuthorizationUserToGroupAsync(dto.Group.Id, authorUserInfo.UserId, Role.Teacher);
            }
            await _repository.DeleteStudentRatingAsync(id);
        }

        public async Task<List<StudentRatingDto>> GetAllStudentRatingsAsync()
        {
            return await _repository.SelectAllStudentRatingsAsync();
        }

        public async Task<List<StudentRatingDto>> GetStudentRatingByUserIdAsync(int userId)
        {
            await _userValidationHelper.GetUserByIdAndThrowIfNotFoundAsync(userId);
            return await _repository.SelectStudentRatingByUserIdAsync(userId);
        }

        public async Task<List<StudentRatingDto>> GetStudentRatingByGroupIdAsync(int groupId, UserIdentityInfo authorUserInfo)
        {
            if (!authorUserInfo.IsAdmin() && !authorUserInfo.IsManager())
            {
                await _userValidationHelper.CheckAuthorizationUserToGroupAsync(groupId, authorUserInfo.UserId, Role.Teacher);
            }
            await _groupValidationHelper.CheckGroupExistenceAsync(groupId);

            return await _repository.SelectStudentRatingByGroupIdAsync(groupId);
        }

        public async Task<StudentRatingDto> UpdateStudentRatingAsync(int id, int value, int periodNumber, UserIdentityInfo authorUserInfo)
        {
            var dto = await _ratingValidationHelper.CheckRaitingExistenceAndReturnDtoAsync(id);
            if (!authorUserInfo.IsAdmin())
            {
                await _userValidationHelper.CheckAuthorizationUserToGroupAsync(dto.Group.Id, authorUserInfo.UserId, Role.Teacher);
            }

            dto = new StudentRatingDto
            {
                Id = id,
                Rating = value,
                ReportingPeriodNumber = periodNumber
            };

            await _repository.UpdateStudentRatingAsync(dto);
            return await _repository.SelectStudentRatingByIdAsync(id);
        }
    }
}