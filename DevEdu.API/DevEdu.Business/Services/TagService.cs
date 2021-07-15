using DevEdu.DAL.Models;
using DevEdu.DAL.Repositories;
using System.Collections.Generic;

namespace DevEdu.Business.Services
{
    public class TagService : ITagService
    {
        private readonly ITagRepository _repository;

        public TagService(ITagRepository repository)
        {
            _repository = repository;
        }

        public int AddTag(TagDto dto) => _repository.AddTag(dto);

        public void DeleteTag(int id) => _repository.DeleteTag(id);

        public void UpdateTag(TagDto dto) => _repository.UpdateTag(dto);

        public List<TagDto> GetAllTags() => _repository.SelectAllTags();

        public TagDto GetTagById(int id) => _repository.SelectTagById(id);

        public int AddTagToTopic(int topicId, int tagId) => _repository.AddTagToTopic(topicId, tagId);

        public int DeleteTagFromTopic(int topicId, int tagId) => _repository.DeleteTagFromTopic(topicId, tagId);
    }
}