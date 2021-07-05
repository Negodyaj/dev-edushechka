using Dapper;
using DevEdu.DAL.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevEdu.DAL.Repositories
{
    public class TagRepository : BaseRepository, ITagRepository 
    {
        public int AddTag(TagDto tagDto)
        {
            return _dbConnection.QuerySingleOrDefault<int>("[dbo].[Tag_Insert]", new { tagDto.Name }, commandType: CommandType.StoredProcedure);
        }
        public void DeleteTag(int id)
        {
            _dbConnection.Execute("[dbo].[Tag_Delete]", new { id }, commandType: CommandType.StoredProcedure);
        }
        public List<TagDto> SelectAllTags()
        {
            List<TagDto> tagDtos = _dbConnection.Query<TagDto>("[dbo].[Tag_SelectAll]", commandType: CommandType.StoredProcedure).ToList();

            return tagDtos;
        }

        public TagDto SelectTagById(int id)
        {
            TagDto tagDto = _dbConnection.QuerySingleOrDefault<TagDto>("[dbo].[Tag_SelectByID]", new { id }, commandType: CommandType.StoredProcedure);

            return tagDto;
        }
        public void UpdateTag(TagDto tagDto)
        {
            _dbConnection.Execute("[dbo].[Tag_Update]", new { tagDto.Id, tagDto.Name }, commandType: CommandType.StoredProcedure);
        }
    }
}
