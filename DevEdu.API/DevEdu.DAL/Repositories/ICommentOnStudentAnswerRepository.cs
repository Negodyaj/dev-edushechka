using DevEdu.DAL.Models;

namespace DevEdu.DAL.Repositories
{
    public interface ICommentOnStudentAnswerRepository
    {
        void AddCommentOnStudentAnswer(int taskId, int studentId, CommentOnStudentAnswerDto commentOnStudentAnswer);
    }
}