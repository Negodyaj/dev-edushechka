using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using DevEdu.API.Models.InputModels;
using DevEdu.DAL.Repositories;
using AutoMapper;
using DevEdu.DAL.Models;
using System.ComponentModel;
using Microsoft.AspNetCore.Http;
using DevEdu.Business.Services;
using DevEdu.API.Models.OutputModels;

namespace DevEdu.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TopicController : Controller
    {
        private readonly ITopicService _topicService;
        private readonly IMapper _mapper;

        public TopicController(IMapper mapper, ITopicService topicService)
        {
            _topicService = topicService;
            _mapper = mapper;
        }

        //  api/topic/{id}
        [HttpGet("{id}")]
        [Description("Get topic by id")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        public TopicDto GetTopicById(int id)
        {
            return _topicService.GetTopic(id);
        }

        [HttpGet]
        [Description("Get all topic")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        public List<TopicDto> GetAllTopics()
        {
            return _topicService.GetAllTopics();
        }

        //  api/topic
        [HttpPost]
        [Description("Add topic")]
        [ProducesResponseType(typeof(TopicOutputModel), (StatusCodes.Status201Created))]
        public TopicOutputModel AddTopic([FromBody] TopicInputModel model)
        {
            var dto = _mapper.Map<TopicDto>(model);
            var output = _topicService.AddTopic(dto);
            return _mapper.Map<TopicOutputModel>(output);
        }

        //  api/topic/{id}
        [HttpDelete("{id}")]
        [Description("Delete topic")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public void DeleteTopic(int id)
        {
            _topicService.DeleteTopic(id);
        }

        //  api/topic/{id}
        [HttpPut("{id}")]
        [Description("Update topic")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public void UpdateTopic(int id, [FromBody] TopicInputModel model)
        {
            var dto = _mapper.Map<TopicDto>(model);
            _topicService.UpdateTopic(id, dto);
        }      
    }
}

