using DevEdu.DAL.Models;
using DevEdu.DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevEdu.Business.Servicies
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
    }
}
