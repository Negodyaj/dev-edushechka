using System.ComponentModel;

namespace DevEdu.API.Models.OutputModels
{
    public class LessonGroupOutputModel
    {
        public int Id { get; set; }
        [DefaultValue(false)]
        public bool IsDeleted { get; set; }
    }
}
