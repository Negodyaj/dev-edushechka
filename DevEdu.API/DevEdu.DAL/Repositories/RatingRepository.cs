using Dapper;
using DevEdu.Core;
using DevEdu.DAL.Enums;
using DevEdu.DAL.Models;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace DevEdu.DAL.Repositories
{
    public class RatingRepository : BaseRepository, IRatingRepository
    {
        private const string _studentRatingInsertProcedure = "dbo.StudentRating_Insert";
        private const string _studentRatingDeleteProcedure = "dbo.StudentRating_Delete";
        private const string _studentRatingSelectAllProcedure = "dbo.StudentRating_SelectAll";
        private const string _studentRatingSelectByIDProcedure = "dbo.StudentRating_SelectById";
        private const string _studentRatingSelectByUserIdProcedure = "dbo.StudentRating_SelectByUserId";
        private const string _studentRatingSelectByGroupIdProcedure = "dbo.StudentRating_SelectByGroupId";
        private const string _studentRatingUpdateProcedure = "dbo.StudentRating_Update";
        public RatingRepository(IOptions<DatabaseSettings> options) : base(options) {  }
        public int AddStudentRating(StudentRatingDto studentRatingDto)
        {
            var userId = studentRatingDto.User.Id;
            var groupId = studentRatingDto.Group.Id;
            var ratingTypeId = studentRatingDto.RatingType.Id;
            return _connection.QuerySingleOrDefault<int>(
                _studentRatingInsertProcedure,
                new
                {
                    userId,
                    groupId,
                    ratingTypeId,
                    studentRatingDto.Rating,
                    studentRatingDto.ReportingPeriodNumber
                },
                commandType: CommandType.StoredProcedure
            );
        }

        public void DeleteStudentRating(int id)
        {
            _connection.Execute(
                _studentRatingDeleteProcedure,
                new { id },
                commandType: CommandType.StoredProcedure
            );
        }

        public List<StudentRatingDto> SelectAllStudentRatings()
        {
            return _connection.Query<StudentRatingDto, GroupDto, RatingTypeDto, UserDto, Role, StudentRatingDto>
                (
                _studentRatingSelectAllProcedure,
                (studentRating, group, ratingType, user, role) =>
                {
                    studentRating.RatingType = ratingType;
                    studentRating.User = user;
                    user.Roles = new List<Role> { role };
                    studentRating.Group = group;
                    return studentRating;
                },
                commandType: CommandType.StoredProcedure
                )
                .ToList();
        }

        public StudentRatingDto SelectStudentRatingById(int id)
        {
            return _connection.Query<StudentRatingDto, GroupDto, RatingTypeDto, UserDto, Role, StudentRatingDto>
                (
                _studentRatingSelectByIDProcedure,
                (studentRating, group, ratingType, user, role) =>
                {
                    studentRating.RatingType = ratingType;
                    studentRating.User = user;
                    user.Roles = new List<Role> { role };
                    studentRating.Group = group;
                    return studentRating;
                },
                new { id },
                commandType: CommandType.StoredProcedure
                )
                .FirstOrDefault();
        }

        public List<StudentRatingDto> SelectStudentRatingByUserId(int userId)
        {
            return _connection.Query<StudentRatingDto, GroupDto, RatingTypeDto, UserDto, Role, StudentRatingDto>
                (
                _studentRatingSelectByUserIdProcedure,
                (studentRating, group, ratingType, user, role) =>
                {
                    studentRating.RatingType = ratingType;
                    studentRating.User = user;
                    user.Roles = new List<Role> { role };
                    studentRating.Group = group;
                    return studentRating;
                },
                new { userId },
                commandType: CommandType.StoredProcedure
                )
                .ToList();
        }

        public List<StudentRatingDto> SelectStudentRatingByGroupId(int groupId)
        {
            return _connection.Query<StudentRatingDto, GroupDto, RatingTypeDto, UserDto, Role, StudentRatingDto>
                (
                _studentRatingSelectByGroupIdProcedure,
                (studentRating, group, ratingType, user, role) =>
                {
                    studentRating.RatingType = ratingType;
                    studentRating.User = user;
                    user.Roles = new List<Role> { role };
                    studentRating.Group = group;
                    return studentRating;
                },
                 new { groupId },
                commandType: CommandType.StoredProcedure
                )
                .ToList();
        }

        public void UpdateStudentRating(StudentRatingDto studentRatingDto)
        {
            _connection.Execute(
                _studentRatingUpdateProcedure,
                new
                {
                    studentRatingDto.Id,
                    studentRatingDto.Rating,
                    studentRatingDto.ReportingPeriodNumber
                },
                commandType: CommandType.StoredProcedure
            );
        }


    }
}
