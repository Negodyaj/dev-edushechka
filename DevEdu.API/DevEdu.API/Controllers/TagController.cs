using AutoMapper;
using DevEdu.API.Models.InputModels;
using DevEdu.DAL.Models;
using DevEdu.DAL.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.ComponentModel;

namespace DevEdu.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TagController : Controller
    {
        private TagRepository _repository;

        private readonly IMapper _mapper;
        public TagController(IMapper mapper, TagRepository repository)
        {
            _repository = repository;
            _mapper = mapper;
        }

        // api/tag
        [HttpPost]
        public int AddTag([FromBody] TagInputModel model)
        {
            var dto = _mapper.Map< TagDto>(model);            
            return _repository.AddTag(dto);
        }

        // api/tag/1
        [HttpDelete("{id}")]
        public void DeleteTag(int id)
        {
            _repository.DeleteTag(id);
        }

        // api/tag/1
        [HttpPut("{id}")]
        public void UpdateTag(int id, [FromBody] TagInputModel model)
        {
            var dto = _mapper.Map<TagDto>(model);
            dto.Id = id;
            _repository.UpdateTag(dto);
        }

        // api/tag
        [HttpGet]
        [Description("Returns the list of all tags")]
        [ProducesResponseType(typeof(TagDto), StatusCodes.Status200OK)]
        public List<TagDto> GetAllTags() // change return type to outputModel
        {
             return _repository.SelectAllTags();
        }

        // api/tag/1
        [HttpGet("{id}")]
        public TagDto GetTagById(int id) // change return type to outputModel
        {
            return _repository.SelectTagById(id);
        }
    }
}