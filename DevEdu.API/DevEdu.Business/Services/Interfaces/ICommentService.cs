using DevEdu.DAL.Models;
using System.Collections.Generic;

namespace DevEdu.Business.Services
{
    public interface ICommentService
    {
        CommentDto GetComment(int id);
        List<CommentDto> GetCommentsByUserId(int userId);
        int AddComment(CommentDto dto);
        void DeleteComment(int id);
        CommentDto UpdateComment(int id, CommentDto dto);
    }
}