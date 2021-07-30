using System.Collections.Generic;

namespace DevEdu.API.Models
{
    public class LessonInfoWithCommentsOutputModel : LessonInfoOutputModel
    {
        public List<CommentInfoOutputModel> Comments { get; set; }
    }
}