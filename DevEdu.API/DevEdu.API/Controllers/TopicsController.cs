using AutoMapper;
using DevEdu.API.Common;
using DevEdu.API.Configuration.ExceptionResponses;
using DevEdu.API.Models;
using DevEdu.Business.Services;
using DevEdu.DAL.Enums;
using DevEdu.DAL.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;

namespace DevEdu.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TopicsController : Controller
    {
        private readonly ITopicService _topicService;
        private readonly IMapper _mapper;

        public TopicsController(IMapper mapper, ITopicService topicService)
        {
            _topicService = topicService;
            _mapper = mapper;
        }

        //  api/topics/{id}
        [AuthorizeRoles(Role.Manager, Role.Methodist)]
        [HttpGet("{id}")]
        [Description("Get topic by id")]
        [ProducesResponseType(typeof(TopicOutputModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status404NotFound)]
        public async Task<TopicOutputModel> GetTopicByIdAsync(int id)
        {
            var output = await _topicService.GetTopicAsync(id);
            return _mapper.Map<TopicOutputModel>(output);
        }

        //  api/topics
        [AuthorizeRoles(Role.Manager, Role.Methodist)]
        [HttpGet]
        [Description("Get all topics")]
        [ProducesResponseType(typeof(List<TopicOutputModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status403Forbidden)]
        public async Task<List<TopicOutputModel>> GetAllTopicsAsync()
        {
            var output = await _topicService.GetAllTopicsAsync();
            return _mapper.Map<List<TopicOutputModel>>(output);
        }

        //  api/topics
        [AuthorizeRoles(Role.Manager, Role.Methodist)]
        [HttpPost]
        [Description("Add topic")]
        [ProducesResponseType(typeof(TopicOutputModel), (StatusCodes.Status201Created))]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ValidationExceptionResponse), StatusCodes.Status422UnprocessableEntity)]
        public async Task<ActionResult<TopicOutputModel>> AddTopicAsync([FromBody] TopicInputModel model)
        {
            var dto = _mapper.Map<TopicDto>(model);
            var topicId = await _topicService.AddTopicAsync(dto);
            var output = await GetTopicByIdAsync(topicId);
            return Created(new Uri($"api/Topic/{output.Id}", UriKind.Relative), output);
        }

        //  api/topics/{id}
        [AuthorizeRoles(Role.Methodist, Role.Manager)]
        [HttpDelete("{id}")]
        [Description("Delete topic")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult> DeleteTopicAsync(int id)
        {
           await _topicService.DeleteTopicAsync(id);
            return NoContent();
        }

        //  api/topics/{id}
        [AuthorizeRoles(Role.Methodist, Role.Manager)]
        [HttpPut("{id}")]
        [Description("Update topic")]
        [ProducesResponseType(typeof(TopicOutputModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ValidationExceptionResponse), StatusCodes.Status422UnprocessableEntity)]
        public async Task<TopicOutputModel> UpdateTopicAsync(int id, [FromBody] TopicInputModel model)
        {
            var dto = _mapper.Map<TopicDto>(model);
            var output = await _topicService.UpdateTopicAsync(id, dto);
            return _mapper.Map<TopicOutputModel>(output);
        }
    }
}