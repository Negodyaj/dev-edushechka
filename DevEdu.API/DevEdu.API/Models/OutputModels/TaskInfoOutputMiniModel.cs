using System.ComponentModel;

namespace DevEdu.API.Models.OutputModels
{
    public class TaskInfoOutputMiniModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Links { get; set; }
        public bool IsRequired { get; set; }
        [DefaultValue(false)]
        public bool IsDeleted { get; set; }
    }
}