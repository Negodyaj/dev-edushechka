using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

        //  api/Course/5
        [HttpGet("{id}")]
        public TopicDto GetTopicById(int id)
        {
            return _topicRepository.GetTopic(id);
        }

        [HttpGet("all")]
        public List<TopicDto> GetAllTopic()
        {
            return _topicRepository.GetAllTopic();
        }

        //  api/course
        [HttpPost]
        public int AddTopic([FromBody] TopicInputModel model)
        {
            var dto = _mapper.Map<TopicDto>(model);
            return _topicRepository.AddTopic(dto);
        }

        //  api/course
        [HttpDelete]
        public void DeleteTopic(int id)
        {
            _topicRepository.DeleteTopic(id);
        }

        //  api/course
        [HttpPut]
        public void UpdateTopic(int id, [FromBody] TopicInputModel model)
        {
            var dto = _mapper.Map<TopicDto>(model);
            _topicRepository.UpdateTopic(id, dto);
        }

        

    }
}
