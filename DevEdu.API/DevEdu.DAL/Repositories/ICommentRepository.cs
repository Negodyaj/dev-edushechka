using System.Collections.Generic;
using DevEdu.DAL.Models;

namespace DevEdu.DAL.Repositories
{
    public interface ICommentRepository
    {
        public int AddComment(CommentDto commentDto);
        public void DeleteComment(int id);
        public CommentDto GetComment(int id);
        public List<CommentDto> GetCommentsByUser(int userId);
        public void UpdateComment(CommentDto commentDto);
    }
}