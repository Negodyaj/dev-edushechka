using System.Collections.Generic;
using DevEdu.DAL.Models;

namespace DevEdu.Business.Servicies
{
    public interface ICommentService
    {
        CommentDto GetComment(int id);
        List<CommentDto> GetCommentsByUserId(int userId);
        int AddComment(CommentDto dto);
        void DeleteComment(int id);
        void UpdateComment(int id, CommentDto dto);
    }
}