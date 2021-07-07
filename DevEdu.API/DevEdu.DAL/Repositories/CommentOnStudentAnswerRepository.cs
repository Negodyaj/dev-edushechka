using Dapper;
using DevEdu.DAL.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevEdu.DAL.Repositories
{
    public class CommentOnStudentAnswerRepository : BaseRepository, ICommentOnStudentAnswerRepository
    {
        private const string _commentInsert = "dbo.Comment_Insert";
        private const string _taskStudentIdByTaskIdAndStudentId = "dbo.Task_Student_GetIdByTaskAndStudent";
        private const string _taskStudentCommentInsert = "dbo.Task_Student_Comment_Insert";

        public CommentOnStudentAnswerRepository()
        {

        }

        public void AddCommentOnStudentAnswer(int taskId, int studentId, CommentOnStudentAnswerDto commentOnStudentAnswer)
        {
            int commentId = _connection.QuerySingle<int>(
                _commentInsert,
                new
                {
                    commentOnStudentAnswer.UserId,
                    commentOnStudentAnswer.Text
                },
                commandType: CommandType.StoredProcedure
            );

            int taskStudentId = _connection.QuerySingle<int>(
                _taskStudentIdByTaskIdAndStudentId,
                new
                {
                    taskId,
                    studentId
                },
                commandType: CommandType.StoredProcedure
           );

            _connection.Query(
                _taskStudentCommentInsert,
                new
                {
                    taskStudentId,
                    commentId
                },
                commandType: CommandType.StoredProcedure
           );
        }
    }
}
