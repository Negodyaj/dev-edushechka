using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Dapper;
using DevEdu.DAL.Models;


namespace DevEdu.DAL.Repositories
{
  public  class TopicRepository : BaseRepository, ITopicRepository
    {
        
        public int AddTopic(TopicDto topicDto)
        {
            return _connection.QuerySingle<int>(
                 "dbo.Topic_Insert",
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
              "dbo.Topic_Delete",
              new { id },
              commandType: CommandType.StoredProcedure
          );


        }

        public TopicDto GetTopic(int id)
        {

            return _connection.QuerySingle<TopicDto>(
              "dbo.Topict_SelectById",
              new { id },
              commandType: CommandType.StoredProcedure
          );


        }

        public List<TopicDto> GetAllTopic()
        {

            return _connection
              .Query<TopicDto>(
                  "dbo.Topic_SelectAll",
                  commandType: CommandType.StoredProcedure
              )
              .AsList();


        }

        public void UpdateTopic(int id, TopicDto topicDto)
        {
            _connection.Execute(
                "dbo.Topic_Update",
                new
                {
                    id,
                    topicDto.Name,
                    topicDto.Duration
                },
                commandType: CommandType.StoredProcedure
            );



        }
    }
}