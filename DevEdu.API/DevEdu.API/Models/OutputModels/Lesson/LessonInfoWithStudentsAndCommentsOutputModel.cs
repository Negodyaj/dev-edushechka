using System.Collections.Generic;
using System.ComponentModel;


namespace DevEdu.API.Models.OutputModels
{
    public class LessonInfoWithStudentsAndCommentsOutputModel : LessonInfoOutputModel
    {
        public List<CommentInfoOutputModel> Comments { get; set; }
        public List<StudentLessonOutputModel> Students { get; set; }

    }
}
