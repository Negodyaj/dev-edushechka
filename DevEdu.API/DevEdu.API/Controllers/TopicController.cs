using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.ComponentModel;
using DevEdu.API.Models.InputModels;
using DevEdu.DAL.Repositories;
using AutoMapper;
using DevEdu.API.Common;
using DevEdu.DAL.Models;
using DevEdu.Business.Services;
using DevEdu.DAL.Enums;
using Microsoft.AspNetCore.Http;

namespace DevEdu.API.Controllers
{
    [AuthorizeRoles()]
    [ApiController]
    [Route("api/[controller]")]
    public class TopicController : Controller
    {
        private readonly ITopicRepository _topicRepository;
        private readonly ITopicService _topicService;
        private readonly IMapper _mapper;

        public TopicController(IMapper mapper, ITopicRepository topicRepository, ITopicService topicService)
        {
            _topicRepository = topicRepository;
            _topicService = topicService;
            _mapper = mapper;
        }

        //  api/topic/{id}
        [HttpGet("{id}")]
        public TopicDto GetTopicById(int id)
        {
            return _topicService.GetTopic(id);
        }

        [HttpGet]
        public List<TopicDto> GetAllTopics()
        {
            return _topicService.GetAllTopics();
        }

        //  api/topic
        [HttpPost]
        public int AddTopic([FromBody] TopicInputModel model)
        {
            var dto = _mapper.Map<TopicDto>(model);
            return _topicService.AddTopic(dto);
        }

        //  api/topic/{id}
        [HttpDelete("{id}")]
        public void DeleteTopic(int id)
        {
            _topicService.DeleteTopic(id);
        }

        //  api/topic/{id}
        [HttpPut("{id}")]
        public void UpdateTopic(int id, [FromBody] TopicInputModel model)
        {
            var dto = _mapper.Map<TopicDto>(model);
           
            _topicService.UpdateTopic(id, dto);
        }

        //  api/topic/{topicId}/tag/{tagId}
        [AuthorizeRoles(Role.Methodist, Role.Teacher)]
        [HttpPost("{topicId}/tag/{tagId}")]
        [Description("Add tag to topic")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public int AddTagToTopic(int topicId, int tagId)
        {
            return _topicService.AddTagToTopic(topicId, tagId);
        }

        //  api/opic/{topicId}/tag/{tagId}
        [AuthorizeRoles(Role.Methodist, Role.Teacher)]
        [HttpDelete("{topicId}/tag/{tagId}")]
        [Description("Delete tag from topic")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public int DeleteTagFromTopic(int topicId, int tagId)
        {
            return _topicService.DeleteTagFromTopic(topicId, tagId);
        }
    }
}
