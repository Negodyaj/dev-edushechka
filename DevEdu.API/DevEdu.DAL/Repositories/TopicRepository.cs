﻿using System.Collections.Generic;
using System.Data;
using Dapper;
using DevEdu.DAL.Models;

namespace DevEdu.DAL.Repositories
{
    public class TopicRepository : BaseRepository, ITopicRepository
    {
        private const string _topicInsertProcedure = "dbo.Topic_Insert";
        private const string _topicDeleteProcedure = "dbo.Topic_Delete";
        private const string _topicSelectByIdProcedure = "dbo.Topic_SelectById";
        private const string _topicSelectAllProcedure = "dbo.Topic_SelectAll";
        private const string _topicUpdateProcedure = "dbo.Topic_Update";     
        private const string _addTopicToCourseProcedure = "[dbo].[Course_Topic_Insert]";
        private const string _deleteTopicToCourseProcedure = "[dbo].[Course_Topic_Delete]";  

        public TopicRepository() { }

        public int AddTopic(TopicDto topicDto)
        {
            return _connection.QuerySingle<int>(
                _topicInsertProcedure,
                 new
                 {
                     topicDto.Name,
                     topicDto.Duration
                 },
                 commandType: CommandType.StoredProcedure
             );
        }

        public void DeleteTopic(int id)  
        {
             _connection.Execute(
              _topicDeleteProcedure,
              new { id },
              commandType: CommandType.StoredProcedure
          );       
            }

        public TopicDto GetTopic(int id)
        {
            return _connection.QuerySingleOrDefault<TopicDto>(   
              _topicSelectByIdProcedure,
              new { id },
              commandType: CommandType.StoredProcedure
          );
        }

        public List<TopicDto> GetAllTopics()
        {
            return _connection
              .Query<TopicDto>(
                  _topicSelectAllProcedure,
                  commandType: CommandType.StoredProcedure
              )
              .AsList();
        }

        public void UpdateTopic(int id, TopicDto topicDto)       
        {
            _connection.Execute(
                _topicUpdateProcedure,
                new
                {
                    id,
                    topicDto.Name,
                    topicDto.Duration
                },
                commandType: CommandType.StoredProcedure
            );
        }
        public int AddTopicToCourse(CourseTopicDto dto)
        {
            return _connection.QuerySingle<int>(
                _addTopicToCourseProcedure,
                new { CourseId = dto.Course.Id, TopicId = dto.Topic.Id, dto.Position },
                commandType: CommandType.StoredProcedure
                );
        }
        public void DeleteTopicFromCourse(int courseId, int topicId)
        {
            _connection.Execute(
                _deleteTopicToCourseProcedure,
                new { courseId, topicId },
                commandType: CommandType.StoredProcedure
                );
        }
    }
}