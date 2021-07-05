using DevEdu.DAL.Models;
using System.Collections.Generic;

namespace DevEdu.DAL.Repositories
{
    public interface ITagRepository
    {
        int AddTag(TagDto tagDto);
        void DeleteTag(int id);
        List<TagDto> SelectAllTags();
        TagDto SelectTagById(int id);
        void UpdateTag(TagDto tagDto);
    }
}