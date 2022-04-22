using DevEdu.DAL.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DevEdu.DAL.Repositories
{
    public interface ICommentRepository
    {
        Task<int> AddCommentAsync(CommentDto commentDto);
        Task DeleteCommentAsync(int id);
        Task<CommentDto> GetCommentAsync(int id);
        Task UpdateCommentAsync(CommentDto commentDto);
    }
}