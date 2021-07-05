using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevEdu.DAL.Repositories
{
    public class User_GroupRepository: BaseRepository
    {
        public void AddTag(int groupId, int userId, int roleId)
        {
            _dbConnection.Execute("[dbo].[User_Group_Insert]", new { groupId, userId, roleId }, commandType: CommandType.StoredProcedure);
        }
        public void DeleteTag(int userId, int groupId)
        {
            _dbConnection.Execute("[dbo].[Tag_Delete]", new { userId, groupId }, commandType: CommandType.StoredProcedure);
        }
    }
}
