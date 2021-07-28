using DevEdu.Business.Constants;
using DevEdu.Business.Exceptions;
using DevEdu.Business.ValidationHelpers;
using DevEdu.DAL.Enums;
using DevEdu.DAL.Models;
using DevEdu.DAL.Repositories;
using System;
using System.Collections.Generic;

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

        public int AddStudentRating(StudentRatingDto studentRatingDto, int authorUserId)
        {
            _groupValidationHelper.CheckGroupExistence(studentRatingDto.Group.Id);
            _groupValidationHelper.CheckTeacherBelongToGroup(studentRatingDto.Group.Id, Convert.ToInt32(authorUserId), Role.Teacher);
            _userValidationHelper.CheckUserExistence(studentRatingDto.User.Id);
            _groupValidationHelper.CheckUserBelongToGroup(studentRatingDto.Group.Id, studentRatingDto.User.Id, Role.Student);
            return _repository.AddStudentRating(studentRatingDto);
        }

        public void DeleteStudentRating(int id, int authorUserId)
        {
            var dto = GetStudentRatingById(id);
            _groupValidationHelper.CheckTeacherBelongToGroup(dto.Group.Id, Convert.ToInt32(authorUserId), Role.Teacher);
            _repository.DeleteStudentRating(id);
        }

        public List<StudentRatingDto> GetAllStudentRatings() => _repository.SelectAllStudentRatings();

        public StudentRatingDto GetStudentRatingById(int id)
        {
            var dto = _repository.SelectStudentRatingById(id);
            if (dto == default)
            {
                throw new EntityNotFoundException(string.Format(ServiceMessages.EntityNotFoundMessage, nameof(dto), id));
            }
            return dto;
        }

        public List<StudentRatingDto> GetStudentRatingByUserId(int userId) => _repository.SelectStudentRatingByUserId(userId);

        public List<StudentRatingDto> GetStudentRatingByGroupId(int groupId) => _repository.SelectStudentRatingByGroupId(groupId);

        public StudentRatingDto UpdateStudentRating(int id, int value, int periodNumber, int authorUserId)
        {
            var dto = _repository.SelectStudentRatingById(id);
            if (dto == default)
            {
                throw new EntityNotFoundException(string.Format(ServiceMessages.EntityNotFoundMessage, nameof(dto), id));
            }
            _groupValidationHelper.CheckTeacherBelongToGroup(dto.Group.Id, Convert.ToInt32(authorUserId), Role.Teacher);
            dto = new StudentRatingDto
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
