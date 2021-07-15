using Dapper;
using DevEdu.DAL.Enums;
using DevEdu.DAL.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevEdu.DAL.Repositories
{
    public class RaitingRepository : BaseRepository, IRaitingRepository
    {
        private const string _studentRaitingInsertProcedure = "dbo.StudentRaiting_Insert";
        private const string _studentRaitingDeleteProcedure = "dbo.StudentRaiting_Delete";
        private const string _studentRaitingSelectAllProcedure = "dbo.StudentRaiting_SelectAll";
        private const string _studentRaitingSelectByIDProcedure = "dbo.StudentRaiting_SelectById";
        private const string _studentRaitingSelectByUserIdProcedure = "dbo.StudentRaiting_SelectByUserId";
        private const string _studentRaitingUpdateProcedure = "dbo.StudentRaiting_Update";

        public int AddStudentRaiting(StudentRaitingDto studentRaitingDto)
        {
            var UserId = studentRaitingDto.User.Id;
            var GroupId = studentRaitingDto.Group.Id;
            var raitingTypeId = studentRaitingDto.RaitingType.Id;
            return _connection.QuerySingleOrDefault<int>(
                _studentRaitingInsertProcedure,
                new
                {
                    UserId,
                    GroupId,
                    raitingTypeId,
                    studentRaitingDto.Raiting,
                    studentRaitingDto.ReportingPeriodNumber
                },
                commandType: CommandType.StoredProcedure
            );
        }

        public void DeleteStudentRaiting(int id)
        {
            _connection.Execute(
                _studentRaitingDeleteProcedure,
                new { id },
                commandType: CommandType.StoredProcedure
            );
        }

        public List<StudentRaitingDto> SelectAllStudentRaitings()
        {
            return _connection.Query<StudentRaitingDto, GroupDto, RaitingTypeDto, UserDto, StudentRaitingDto>
                (
                _studentRaitingSelectAllProcedure,
                (studentRaiting, group, raitingType, user) =>
                {
                    studentRaiting.RaitingType = raitingType;
                    studentRaiting.User = user;
                    studentRaiting.Group = group;
                    return studentRaiting;
                },
                commandType: CommandType.StoredProcedure
                )
                .ToList();
        }

        public StudentRaitingDto SelectStudentRaitingById(int id)
        {
            return _connection.Query<StudentRaitingDto, GroupDto, RaitingTypeDto, UserDto, StudentRaitingDto>
                (
                _studentRaitingSelectByIDProcedure,
                (studentRaiting, group, raitingType, user) =>
                {
                    studentRaiting.RaitingType = raitingType;
                    studentRaiting.User = user;
                    studentRaiting.Group = group;
                    return studentRaiting;
                },
                new { id },
                commandType: CommandType.StoredProcedure
                )
                .FirstOrDefault();
        }

        public List<StudentRaitingDto> SelectStudentRaitingByUserId(int userId)
        {
            return _connection.Query<StudentRaitingDto, GroupDto, RaitingTypeDto, UserDto, StudentRaitingDto>
                (
                _studentRaitingSelectByUserIdProcedure,
                (studentRaiting, group, raitingType, user) =>
                {
                    studentRaiting.RaitingType = raitingType;
                    studentRaiting.User = user;
                    studentRaiting.Group = group;
                    return studentRaiting;
                },
                new { userId },
                commandType: CommandType.StoredProcedure
                )
                .ToList();
        }

        public void UpdateStudentRaiting(StudentRaitingDto studentRaitingDto)
        {
            _connection.Execute(
                _studentRaitingUpdateProcedure,
                new
                {
                    studentRaitingDto.Id,
                    studentRaitingDto.Raiting
                },
                commandType: CommandType.StoredProcedure
            );
        }
    }
}
