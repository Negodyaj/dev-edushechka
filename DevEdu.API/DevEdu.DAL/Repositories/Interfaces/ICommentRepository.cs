using System.Collections.Generic;
using DevEdu.DAL.Models;

namespace DevEdu.DAL.Repositories
{
    public interface ICommentRepository
    {
        int AddComment(CommentDto commentDto);
        void DeleteComment(int id);
        CommentDto GetComment(int id);
        void UpdateComment(CommentDto commentDto);
        List<CommentDto> SelectCommentsFromLessonByLessonId(int lessonId);
    }
}