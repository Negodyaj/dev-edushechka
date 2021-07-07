using DevEdu.DAL.Models;
using DevEdu.DAL.Repositories;

namespace DevEdu.Business.Servicies
{
    public class CommentService : ICommentService
    {
        private readonly ICommentRepository _commentRepository;

        public CommentService(ICommentRepository commentRepository)
        {
            _commentRepository = commentRepository;
        }

        public CommentDto GetComment(int id) => _commentRepository.GetComment(id);

        public void UpdateComment(int id, CommentDto dto)
        {
            dto.Id = id;
            _commentRepository.UpdateComment(dto);
        }
    }
}
