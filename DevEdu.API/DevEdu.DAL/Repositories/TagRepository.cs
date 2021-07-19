using Dapper;
using DevEdu.DAL.Models;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace DevEdu.DAL.Repositories
{
    public class TagRepository : BaseRepository, ITagRepository
    {
        private const string _tagInsertProcedure = "dbo.Tag_Insert";
        private const string _tagDeleteProcedure = "dbo.Tag_Delete";
        private const string _tagSelectAllProcedure = "dbo.Tag_SelectAll";
        private const string _tagSelectByIDProcedure = "dbo.Tag_SelectByID";
        private const string _tagUpdateProcedure = "dbo.Tag_Update";



        public int AddTag(TagDto tagDto)
        {
            return _connection.QuerySingleOrDefault<int>(
                _tagInsertProcedure,
                new { tagDto.Name },
                commandType: CommandType.StoredProcedure
            );
        }

        public int DeleteTag(int id)
        {
            return _connection.Execute(
                _tagDeleteProcedure,
                new { id },
                commandType: CommandType.StoredProcedure
            );
        }

        public List<TagDto> SelectAllTags()
        {
            return _connection.Query<TagDto>(
                _tagSelectAllProcedure,
                commandType: CommandType.StoredProcedure
            )
            .ToList();
        }

        public TagDto SelectTagById(int id)
        {
            return _connection.QuerySingleOrDefault<TagDto>(
                _tagSelectByIDProcedure,
                new { id },
                commandType: CommandType.StoredProcedure
            );
        }

        public int UpdateTag(TagDto tagDto)
        {
            return _connection.Execute(
                _tagUpdateProcedure,
                new
                {
                    tagDto.Id,
                    tagDto.Name
                },
                commandType: CommandType.StoredProcedure
            );
        }
    }
}