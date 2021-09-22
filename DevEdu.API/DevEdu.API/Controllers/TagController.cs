using AutoMapper;
using DevEdu.API.Common;
using DevEdu.API.Configuration.ExceptionResponses;
using DevEdu.API.Models;
using DevEdu.Business.Services;
using DevEdu.DAL.Enums;
using DevEdu.DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;

namespace DevEdu.API.Controllers
{
    [Authorize]
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
        [AuthorizeRoles(Role.Teacher, Role.Manager, Role.Methodist)]
        [HttpPost]
        [Description("Add tag to database")]
        [ProducesResponseType(typeof(TagOutputModel), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ValidationExceptionResponse), StatusCodes.Status422UnprocessableEntity)]
        public async Task<ActionResult<TagOutputModel>> AddTagAsync([FromBody] TagInputModel model)
        {
            var dto = _mapper.Map<TagDto>(model);
            dto = await _service.AddTagAsync(dto);
            var output = _mapper.Map<TagOutputModel>(dto);
            return Created(new Uri($"api/Tag/{output.Id}", UriKind.Relative), output);
        }

        // api/tag/1
        [AuthorizeRoles(Role.Teacher, Role.Manager, Role.Methodist)]
        [HttpDelete("{id}")]
        [Description("Delete tag from database")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult> DeleteTagAsync(int id)
        {
            await _service.DeleteTagAsync(id);
            return NoContent();
        }

        // api/tag/1
        [AuthorizeRoles(Role.Teacher, Role.Manager, Role.Methodist)]
        [HttpPut("{id}")]
        [Description("Update tag in database and return updated tag")]
        [ProducesResponseType(typeof(TagOutputModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ValidationExceptionResponse), StatusCodes.Status422UnprocessableEntity)]
        public async Task<TagOutputModel> UpdateTagAsync(int id, [FromBody] TagInputModel model)
        {
            var dto = _mapper.Map<TagDto>(model);
            dto = await _service.UpdateTagAsync(dto, id);
            return _mapper.Map<TagOutputModel>(dto);
        }

        // api/tag
        [HttpGet]
        [Description("Get all tags from database")]
        [ProducesResponseType(typeof(List<TagOutputModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status403Forbidden)]
        public async Task<List<TagOutputModel>> GetAllTagsAsync()
        {
            var queryResult = await _service.GetAllTagsAsync();
            return _mapper.Map<List<TagOutputModel>>(queryResult);
        }

        // api/tag/{id}
        [HttpGet("{id}")]
        [Description("Get tag from database by id")]
        [ProducesResponseType(typeof(TagOutputModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status404NotFound)]
        public async Task<TagOutputModel> GetTagByIdAsync(int id)
        {
            var dto = await _service.GetTagByIdAsync(id);
            return _mapper.Map<TagOutputModel>(dto);
        }
    }
}