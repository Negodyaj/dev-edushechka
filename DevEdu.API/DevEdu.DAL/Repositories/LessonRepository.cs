using System;
using Dapper;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using DevEdu.DAL.Models;

namespace DevEdu.DAL.Repositories
{
    class LessonRepository
    {
        string connectionString = "Data Source=80.78.240.16;Initial Catalog = DevEdu; Persist Security Info=True;User ID = student;Password=qwe!23; Pooling=False;MultipleActiveResultSets=False;Connect Timeout = 60; Encrypt=False;TrustServerCertificate=False";

        private readonly IDbConnection _connection;

        public LessonRepository()
        {
            _connection = new SqlConnection(connectionString);
        }

        public int AddLesson(LessonDto lessonDto)
        {
            return _connection.QueryFirst<int>("dbo.Lesson_Insert", new
            {
                lessonDto.Date,
                lessonDto.TeacherComment,
                lessonDto.TeacherId
            },
            commandType: CommandType.StoredProcedure);
        }

        public int DeleteLesson(int id)
        {
            return _connection.QueryFirst<int>("dbo.Lesson_Delete", new { id }, commandType: CommandType.StoredProcedure);
        }

        public List<LessonDto> SelectAllLessons()
        {
            return _connection.Query<LessonDto>("dbo.Lesson_SelectAll", commandType: CommandType.StoredProcedure).AsList<LessonDto>();
        }

        public LessonDto SelectLessonById(int id)
        {
            return _connection.QueryFirst<LessonDto>("dbo.Lesson_SelectById", new { id}, commandType: CommandType.StoredProcedure);
        }

        public int UpdateLesson(int id, String commentDto, DateTime date)
        {
            return _connection.QueryFirst<int>("dbo.Lesson_Update", new
            {
                id, 
                commentDto,
                date
            }, 
            commandType: CommandType.StoredProcedure);
        }
        

        
    }
}


