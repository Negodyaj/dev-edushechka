using System.Collections.Generic;
using System.ComponentModel;


namespace DevEdu.API.Models.OutputModels
{
    public class LessonInfoWithStudentsAndCommentsOutputModel : LessonInfoOutputModel
    {
        public List<CommentInfoOutputModel> Comments { get; set; }
        public List<UserInfoOutputModel> Students { get; set; }
        [DefaultValue(false)]
        public bool IsDeleted { get; set; }
    }
}
