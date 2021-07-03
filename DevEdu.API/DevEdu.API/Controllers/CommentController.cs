﻿using System.Collections.Generic;
using AutoMapper;
using DevEdu.API.Models.InputModels;
using DevEdu.DAL.Models;
using DevEdu.DAL.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace DevEdu.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CommentController : Controller
    {
        private readonly ICommentRepository _commentRepository;
        private readonly IMapper _mapper;
        public CommentController(IMapper mapper, ICommentRepository commentRepository)
        {
            _commentRepository = commentRepository;
            _mapper = mapper;
        }

        //  api/comment/5
        [HttpGet("{id}")]
        public CommentDto GetComment(int id)
        {
            return _commentRepository.GetComment(id);
        }

        //  api/comment/by-user/1
        [HttpGet("by-user/{userId}")]
        public List<CommentDto> GetAllCommentsByUserId(int userId)
        {
            return _commentRepository.GetCommentsByUser(userId);
        }

        //  api/comment
        [HttpPost]
        public int AddComment([FromBody] CommentAddtInputModel model)
        {
            var dto = _mapper.Map<CommentDto>(model);
            return _commentRepository.AddComment(dto);
        }

        //  api/comment/5
        [HttpDelete("{id}")]
        public void DeleteComment(int id)
        {
            _commentRepository.DeleteComment(id);
        }

        //  api/comment/5
        [HttpPut("{id}")]
        public string UpdateComment(int id, [FromBody] CommentUpdatetInputModel model)
        {
            var dto = _mapper.Map<CommentDto>(model);
            _commentRepository.UpdateComment(id, dto);
            return $"Text comment №{id} change to {model.Text}";
        }
    }
}