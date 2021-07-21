using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DevEdu.API.Models.OutputModels
{
    public class LessonInfoWithCommentsOutputModel : LessonInfoOutputModel
    {
        public List<CommentInfoOutputModel> Comments { get; set; }
    }
}
