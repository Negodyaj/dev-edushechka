using System.Collections.Generic;
using DevEdu.DAL.Models;

namespace DevEdu.Business.Services
{
    public interface ICommentService
    {
        CommentDto GetComment(int id);
        List<CommentDto> GetCommentsByUserId(int userId);
        int AddComment(CommentDto dto);
        int DeleteComment(int id);
        int UpdateComment(int id, CommentDto dto);
    }
}