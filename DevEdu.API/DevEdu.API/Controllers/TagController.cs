using AutoMapper;
using DevEdu.API.Models.InputModels;
using DevEdu.API.Models.OutputModels;
using DevEdu.Business.Services;
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
        private readonly ITagService _service;
        private readonly IMapper _mapper;

        public TagController(IMapper mapper, ITagService service)
        {
            _service = service;
            _mapper = mapper;
        }

        // api/tag
        [HttpPost]
        [Description("Add tag to database")]
        [ProducesResponseType(typeof(int), StatusCodes.Status201Created)]
        public int AddTag([FromBody] TagInputModel model)
        {
            var dto = _mapper.Map< TagDto>(model);            
            return _service.AddTag(dto);
        }

        // api/tag/1
        [HttpDelete("{id}")]
        [Description("Soft delete tag from database")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public void DeleteTag(int id) => _service.DeleteTag(id);

        // api/tag/1
        [HttpPut("{id}")]
        [Description("Update tag in database and return updated tag")]
        [ProducesResponseType(typeof(TagOutputModel), StatusCodes.Status200OK)]
        public TagOutputModel UpdateTag(int id, [FromBody] TagInputModel model)
        {
            var dto = _mapper.Map<TagDto>(model);
            dto = _service.UpdateTag(dto, id);
            return _mapper.Map<TagOutputModel>(dto);
        }

        // api/tag
        [HttpGet]
        [Description("Get all tags from database")]
        [ProducesResponseType(typeof(List<TagOutputModel>), StatusCodes.Status200OK)]
        public List<TagOutputModel> GetAllTags()
        {
            List<TagDto> queryResult = _service.GetAllTags();
            return _mapper.Map<List<TagOutputModel>>(queryResult);
        }

        // api/tag/1
        [HttpGet("{id}")]
        [Description("Get tag from database by ID")]
        [ProducesResponseType(typeof(TagOutputModel), StatusCodes.Status200OK)]
        public TagOutputModel GetTagById(int id)
        {
            TagDto queryResult = _service.GetTagById(id);
            return _mapper.Map<TagOutputModel>(queryResult);
        }
    }
}