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
    public class RaitingRepository : BaseRepository
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
            var raitingTypeId = studentRaitingDto.RaitingType.Id;
            return _connection.QuerySingleOrDefault<int>(
                _studentRaitingInsertProcedure,
                new
                {
                    UserId,
                    raitingTypeId,
                    studentRaitingDto.Raiting
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
            Dictionary<int, StudentRaitingDto> result = new Dictionary<int, StudentRaitingDto>();
            _connection.Query<StudentRaitingDto, RaitingTypeDto, UserDto, Role, StudentRaitingDto>
                (
                _studentRaitingSelectAllProcedure,
                (studentRaiting, raitingType, user, role) =>
                {
                    if (result.TryAdd(studentRaiting.Id, studentRaiting))
                    {
                        studentRaiting.RaitingType = raitingType;
                        studentRaiting.User = user;
                        user.Roles = new List<Role> { role };
                    }
                    else result[studentRaiting.Id].User.Roles.Add(role);
                    return studentRaiting;
                },
                commandType: CommandType.StoredProcedure
                );
            return result.Values.ToList();
        }

        public StudentRaitingDto SelectStudentRaitingById(int id)
        {
            StudentRaitingDto result = default;
            _connection.Query<StudentRaitingDto, RaitingTypeDto, UserDto, Role, StudentRaitingDto>
                (
                _studentRaitingSelectByIDProcedure,
                (studentRaiting, raitingType, user, role) =>
                {
                    if (result == null)
                    {
                        result = studentRaiting;
                        result.RaitingType = raitingType;
                        result.User = user;
                        user.Roles = new List<Role> { role };
                    }
                    else result.User.Roles.Add(role);
                    return studentRaiting;
                },
                new { id },
                commandType: CommandType.StoredProcedure
                );
            return result;
        }

        public StudentRaitingDto SelectStudentRaitingByUserId(int userId)
        {
            StudentRaitingDto result = default;
            _connection.Query<StudentRaitingDto, RaitingTypeDto, UserDto, Role, StudentRaitingDto>
                (
                _studentRaitingSelectByUserIdProcedure,
                (studentRaiting, raitingType, user, role) =>
                {
                    if (result == null)
                    {
                        result = studentRaiting;
                        result.RaitingType = raitingType;
                        result.User = user;
                        user.Roles = new List<Role> { role };
                    }
                    else result.User.Roles.Add(role);
                    return studentRaiting;
                },
                new { userId },
                commandType: CommandType.StoredProcedure
                );
            return result;
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
