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
        private const string _tagInsertProcedure = "[dbo].[Tag_Insert]";
        private const string _tagDeleteProcedure = "[dbo].[Tag_Delete]";
        private const string _tagSelectAllProcedure = "[dbo].[Tag_SelectAll]";
        private const string _tagSelectByIDProcedure = "[dbo].[Tag_SelectByID]";
        private const string _tagUpdateProcedure = "[dbo].[Tag_Update]";
        public int AddTag(TagDto tagDto)
        {
            return _connection.QuerySingleOrDefault<int>(_tagInsertProcedure, new { tagDto.Name }, commandType: CommandType.StoredProcedure);
        }
        public void DeleteTag(int id)
        {
            _connection.Execute(_tagDeleteProcedure, new { id }, commandType: CommandType.StoredProcedure);
        }
        public List<TagDto> SelectAllTags()
        {
            List<TagDto> tagDtos = _connection.Query<TagDto>(_tagSelectAllProcedure, commandType: CommandType.StoredProcedure).ToList();

            return tagDtos;
        }

        public TagDto SelectTagById(int id)
        {
            TagDto tagDto = _connection.QuerySingleOrDefault<TagDto>(_tagSelectByIDProcedure, new { id }, commandType: CommandType.StoredProcedure);

            return tagDto;
        }
        public void UpdateTag(TagDto tagDto)
        {
            _connection.Execute(_tagUpdateProcedure, new { tagDto.Id, tagDto.Name }, commandType: CommandType.StoredProcedure);
        }
    }
}
