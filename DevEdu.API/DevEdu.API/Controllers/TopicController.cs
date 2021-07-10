using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using DevEdu.API.Models.InputModels;
using DevEdu.DAL.Repositories;
using AutoMapper;
using DevEdu.DAL.Models;
using DevEdu.Business.Servicies;

namespace DevEdu.API.Controllers
{
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
    }
}
