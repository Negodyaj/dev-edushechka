using System.Collections.Generic;

namespace DevEdu.DAL.Models
{
    public class MaterialDto : BaseDto
    {
        public string Content { get; set; }
        public List<TagDto> Tags { get; set; }
    }
}