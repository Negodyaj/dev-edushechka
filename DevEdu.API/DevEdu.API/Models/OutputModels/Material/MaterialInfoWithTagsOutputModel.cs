using System.Collections.Generic;

namespace DevEdu.API.Models.OutputModels
{
    public class MaterialInfoWithTagsOutputModel : MaterialInfoOutputModel
    {
        public List<TagInfoOutputModel> Tags { get; set; }
    }
}
