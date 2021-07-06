using System.Collections.Generic;
using DevEdu.DAL.Models;
using DevEdu.DAL.Repositories;

namespace DevEdu.Business.Services
{
    public class CommentService : ICommentService
    {
        private readonly ICommentRepository _commentRepository;

        public CommentService(ICommentRepository commentRepository)
        {
            _commentRepository = commentRepository;
        }

        public CommentDto GetComment(int id) => _commentRepository.GetComment(id);

        public List<CommentDto> GetCommentsByUserId(int userId) => _commentRepository.GetCommentsByUserId(userId);

        public int AddComment(CommentDto dto) => _commentRepository.AddComment(dto);

        public void DeleteComment(int id) => _commentRepository.DeleteComment(id);

        public void UpdateComment(int id, CommentDto dto)
        {
            dto.Id = id;
            _commentRepository.UpdateComment(dto);
        }
    }
}