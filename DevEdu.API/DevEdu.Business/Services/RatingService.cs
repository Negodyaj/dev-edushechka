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

        public StudentRatingDto AddStudentRating(StudentRatingDto studentRatingDto, int authorUserId)
        {
            _groupValidationHelper.CheckGroupExistence(studentRatingDto.Group.Id);
            _groupValidationHelper.CheckTeacherBelongToGroup(studentRatingDto.Group.Id, Convert.ToInt32(authorUserId), Role.Teacher);
            _userValidationHelper.CheckUserExistence(studentRatingDto.User.Id);
            _groupValidationHelper.CheckUserBelongToGroup(studentRatingDto.Group.Id, studentRatingDto.User.Id, Role.Student);
            var id = _repository.AddStudentRating(studentRatingDto);
            return _repository.SelectStudentRatingById(id);
        }

        public void DeleteStudentRating(int id, int authorUserId)
        {
            var dto = _ratingValidationHelper.CheckRaitingExistence(id);
            _groupValidationHelper.CheckTeacherBelongToGroup(dto.Group.Id, Convert.ToInt32(authorUserId), Role.Teacher);
            _repository.DeleteStudentRating(id);
        }

        public List<StudentRatingDto> GetAllStudentRatings()
        {
            return _repository.SelectAllStudentRatings();
        }

        public List<StudentRatingDto> GetStudentRatingByUserId(int userId)
        {
            _userValidationHelper.CheckUserExistence(userId);
            return _repository.SelectStudentRatingByUserId(userId);
        }

        public List<StudentRatingDto> GetStudentRatingByGroupId(int groupId, int authorUserId, List<Role> authRoles)
        {
            if (!(authRoles.Contains(Role.Manager) || authRoles.Contains(Role.Admin)))
            {
                _groupValidationHelper.CheckTeacherBelongToGroup(groupId, Convert.ToInt32(authorUserId), Role.Teacher);
            }
            _groupValidationHelper.CheckGroupExistence(groupId);
            return _repository.SelectStudentRatingByGroupId(groupId);
        }

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
            return _repository.SelectStudentRatingById(id);
        }
    }
}
