﻿using System;
using Dapper;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using DevEdu.DAL.Models;

namespace DevEdu.DAL.Repositories
{
    public class LessonRepository: BaseRepository
    {
        public int AddLesson(LessonDto lessonDto)
        {
            return _connection.QueryFirst<int>(
                "dbo.Lesson_Insert",
                new
                {
                    lessonDto.Date,
                    lessonDto.TeacherComment,
                    lessonDto.TeacherId
                },
                commandType: CommandType.StoredProcedure
            );
        }

        public void DeleteLesson(int id)
        {
            _connection.Execute(
                "dbo.Lesson_Delete", 
                new { id },
                commandType: CommandType.StoredProcedure
            );
        }

        public List<LessonDto> SelectAllLessons()
        {
            return _connection
                .Query<LessonDto>(
                    "dbo.Lesson_SelectAll", 
                    commandType: CommandType.StoredProcedure
                )
                .AsList<LessonDto>();
        }

        public LessonDto SelectLessonById(int id)
        {
            return _connection.QuerySingleOrDefault<LessonDto>(
                "dbo.Lesson_SelectById", 
                new { id }, 
                commandType: CommandType.StoredProcedure
            );
        }

        public int UpdateLesson(int id, String commentDto, DateTime date)
        {
            return _connection.QuerySingleOrDefault<int>(
                "dbo.Lesson_Update", 
                new
                {
                    id, 
                    commentDto,
                    date
                }, 
                commandType: CommandType.StoredProcedure
            );
        }

        public void AddTopicToLesson(int lessonId, int topicId)
        {
            _connection.Query("dbo.Lesson_Topic_Insert", new { lessonId, topicId });
        }


        public void DeleteTopicFromLesson(int lessonId, int topicId)
        {
            _connection.Query("dbo.Lesson_Topic_Delete", new { lessonId, topicId });
        }

        public int AddCommentToLesson(int lessonId, int commentId)
        {
            return _connection.QueryFirst<int>(
                "dbo.Lesson_Comment_Insert", 
                new 
                { 
                    lessonId, 
                    commentId 
                }, 
                commandType: CommandType.StoredProcedure
            );
        }

        public void DeleteCommentFromLesson(int lessonId, int commentId)
        {
            _connection.Execute(
                "dbo.Lesson_Comment_Delete", 
                new
                { 
                    lessonId, 
                    commentId
                },
                commandType: CommandType.StoredProcedure
            );
        }
    }
}

