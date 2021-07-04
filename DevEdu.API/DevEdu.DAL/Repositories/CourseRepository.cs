using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;

namespace DevEdu.DAL.Repositories
{
    public class CourseRepository : BaseRepository, ICourseRepository
    {
        public CourseRepository()
        {
                
        }

        public void AddTagToTopic(int topicId, int tagId)
        {
            string query = "dbo.Tag_Topic_Insert";
            _connection.Query(
                query,
                new 
                {
                    topicId, 
                    tagId
                },
                commandType: CommandType.StoredProcedure
                );
        }

        public void DeleteTagFromTopic(int topicId, int tagId)
        {
            string query = "dbo.Tag_Topic_Delete";
            _connection.Query(
                query,
                new
                {
                    topicId,
                    tagId
                },
                commandType: CommandType.StoredProcedure
                );
        }
    }
}
