using System.Collections.Generic;

namespace DevEdu.API.Models
{
    public class LessonInfoWithStudentsAndCommentsOutputModel : LessonInfoOutputModel
    {
        public List<StudentLessonOutputModel> Students { get; set; }
    }
}