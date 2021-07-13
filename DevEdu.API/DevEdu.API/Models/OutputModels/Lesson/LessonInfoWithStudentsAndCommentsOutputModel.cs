using System.Collections.Generic;
using System.ComponentModel;


namespace DevEdu.API.Models.OutputModels
{
    public class LessonInfoWithStudentsAndCommentsOutputModel : LessonInfoOutputModel
    {
        List<CommentInfoOutputModel> Comments { get; set; }
        List<UserInfoOutputModel> Students { get; set; }
        [DefaultValue(false)]
        public bool IsDeleted { get; set; }
    }
}
