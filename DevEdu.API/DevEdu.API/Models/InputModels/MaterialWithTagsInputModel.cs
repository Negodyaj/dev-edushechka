using System.Collections.Generic;

namespace DevEdu.API.Models.InputModels
{
    public class MaterialWithTagsInputModel : MaterialInputModel
    {
        public List<int> TagsIds { get; set; }
    }
}