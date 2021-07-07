using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using DevEdu.API.Models.InputModels;
using DevEdu.DAL.Repositories;
using AutoMapper;
using DevEdu.DAL.Models;

namespace DevEdu.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TopicController : Controller
    {
        private readonly ITopicRepository _topicRepository;
        private readonly IMapper _mapper;

        public TopicController(IMapper mapper, ITopicRepository topicRepository)
        {
            _topicRepository = topicRepository;
            _mapper = mapper;
        }

        //  api/topic/{id}
        [HttpGet("{id}")]
        public TopicDto GetTopicById(int id)
        {
            return _topicRepository.GetTopic(id);
        }

        [HttpGet]
        public List<TopicDto> GetAllTopics()
        {
            return _topicRepository.GetAllTopics();
        }

        //  api/topic
        [HttpPost]
        public int AddTopic([FromBody] TopicInputModel model)
        {
            var dto = _mapper.Map<TopicDto>(model);
            return _topicRepository.AddTopic(dto);
        }

        //  api/topic/{id}
        [HttpDelete("{id}")]
        public void DeleteTopic(int id)
        {
            _topicRepository.DeleteTopic(id);
        }

        //  api/topic/{id}
        [HttpPut("{id}")]
        public void UpdateTopic(int id, [FromBody] TopicInputModel model)
        {
            var dto = _mapper.Map<TopicDto>(model);
            _topicRepository.UpdateTopic(id, dto);
        }      
    }
}
