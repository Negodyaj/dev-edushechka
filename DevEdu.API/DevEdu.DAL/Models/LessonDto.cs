using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevEdu.DAL.Models
{
    class LessonDto : BaseDto
    {
        public DateTime Date { get; set; }
        public CommentDto TeacherComment { get; set; }
        public int TeacherId { get; set; }
    }
}

