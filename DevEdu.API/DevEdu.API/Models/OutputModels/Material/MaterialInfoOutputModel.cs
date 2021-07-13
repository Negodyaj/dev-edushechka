
using System.ComponentModel;

namespace DevEdu.API.Models.OutputModels
{
    public class MaterialInfoOutputModel
    {
        public int Id { get; set; }
        public string Content { get; set; }
        [DefaultValue(false)]
        public bool IsDeleted { get; set; }
    }
}
