using System.Collections.Generic;
using DevEdu.DAL.Models;

namespace DevEdu.DAL.Repositories
{
    public interface ICommentRepository
    {
        int AddComment(CommentDto commentDto);
        int DeleteComment(int id);
        CommentDto GetComment(int id);
        List<CommentDto> GetCommentsByUser(int userId);
        int UpdateComment(CommentDto commentDto);
    }
}