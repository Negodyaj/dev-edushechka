using System.ComponentModel;

namespace DevEdu.API.Models.OutputModels
{
    public class TagInfoOutputModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        [DefaultValue(false)]
        public bool IsDeleted { get; set; }
    }
}
