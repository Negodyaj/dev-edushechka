using Dapper;
using DevEdu.Core;
using DevEdu.DAL.Enums;
using DevEdu.DAL.Models;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace DevEdu.DAL.Repositories
{
    public class RatingRepository : BaseRepository, IRatingRepository
    {
        private const string _studentRatingInsertProcedure = "dbo.StudentRating_Insert";
        private const string _studentRatingDeleteProcedure = "dbo.StudentRating_Delete";
        private const string _studentRatingSelectAllProcedure = "dbo.StudentRating_SelectAll";
        private const string _studentRatingSelectByIdProcedure = "dbo.StudentRating_SelectById";
        private const string _studentRatingSelectByUserIdProcedure = "dbo.StudentRating_SelectByUserId";
        private const string _studentRatingSelectByGroupIdProcedure = "dbo.StudentRating_SelectByGroupId";
        private const string _studentRatingUpdateProcedure = "dbo.StudentRating_Update";

        public RatingRepository(IOptions<DatabaseSettings> options) : base(options)
        {
        }

        public async Task<int> AddStudentRatingAsync(StudentRatingDto studentRatingDto)
        {
            var userId = studentRatingDto.User.Id;
            var groupId = studentRatingDto.Group.Id;
            var ratingTypeId = studentRatingDto.RatingType.Id;

            return await _connection.QuerySingleOrDefaultAsync<int>(
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

        public async Task DeleteStudentRatingAsync(int id)
        {
            await _connection.ExecuteAsync(
                 _studentRatingDeleteProcedure,
                 new { id },
                 commandType: CommandType.StoredProcedure
             );
        }

        public async Task<List<StudentRatingDto>> SelectAllStudentRatingsAsync()
        {
            return (await _connection
                .QueryAsync<StudentRatingDto, GroupDto, RatingTypeDto, UserDto, Role, StudentRatingDto>(
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
                ))
                .ToList();
        }

        public async Task<StudentRatingDto> SelectStudentRatingByIdAsync(int id)
        {
            return (await _connection
                .QueryAsync<StudentRatingDto, GroupDto, RatingTypeDto, UserDto, Role, StudentRatingDto>(
                _studentRatingSelectByIdProcedure,
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
                ))
                .FirstOrDefault();
        }

        public async Task<List<StudentRatingDto>> SelectStudentRatingByUserIdAsync(int userId)
        {
            return (await _connection
                .QueryAsync<StudentRatingDto, GroupDto, RatingTypeDto, UserDto, Role, StudentRatingDto>(
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
                ))
                .ToList();
        }

        public async Task<List<StudentRatingDto>> SelectStudentRatingByGroupIdAsync(int groupId)
        {
            return (await _connection
                .QueryAsync<StudentRatingDto, GroupDto, RatingTypeDto, UserDto, Role, StudentRatingDto>(
                _studentRatingSelectByGroupIdProcedure, (studentRating, group, ratingType, user, role) =>
                {
                    studentRating.RatingType = ratingType;
                    studentRating.User = user;
                    user.Roles = new List<Role> { role };
                    studentRating.Group = group;
                    return studentRating;
                },
                new { groupId },
                commandType: CommandType.StoredProcedure
                ))
                .ToList();
        }

        public async Task UpdateStudentRatingAsync(StudentRatingDto studentRatingDto)
        {
            await _connection.ExecuteAsync(
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