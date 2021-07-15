using DevEdu.DAL.Models;
using System.Collections.Generic;

namespace DevEdu.DAL.Repositories
{
    public interface ITagRepository
    {
        int AddTag(TagDto tagDto);
        int DeleteTag(int id);
        List<TagDto> SelectAllTags();
        TagDto SelectTagById(int id);
        int UpdateTag(TagDto tagDto);
    }
}