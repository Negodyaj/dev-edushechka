using AutoMapper;
using DevEdu.API.Models.InputModels;
using DevEdu.Business.Servicies;
using DevEdu.DAL.Models;
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
        private ITagService _service;

        private readonly IMapper _mapper;
        public TagController(IMapper mapper, ITagService service)
        {
            _service = service;
            _mapper = mapper;
        }

        // api/tag
        [HttpPost]
        public int AddTag([FromBody] TagInputModel model)
        {
            var dto = _mapper.Map< TagDto>(model);            
            return _service.AddTag(dto);
        }

        // api/tag/1
        [HttpDelete("{id}")]
        public void DeleteTag(int id) => _service.DeleteTag(id);

        // api/tag/1
        [HttpPut("{id}")]
        public void UpdateTag(int id, [FromBody] TagInputModel model)
        {
            var dto = _mapper.Map<TagDto>(model);
            dto.Id = id;
            _service.UpdateTag(dto);
        }

        // api/tag
        [HttpGet]
        public List<TagDto> GetAllTags() => _service.GetAllTags(); // change return type to outputModel

        // api/tag/1
        [HttpGet("{id}")]
        public TagDto GetTagById(int id) => _service.GetTagById(id); // change return type to outputModel
    }
}