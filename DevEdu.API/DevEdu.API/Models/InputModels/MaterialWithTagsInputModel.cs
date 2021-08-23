using System.Collections.Generic;

namespace DevEdu.API.Models
{
    public class MaterialWithTagsInputModel : MaterialInputModel
    {
        public List<int> TagsIds { get; set; }
    }
}